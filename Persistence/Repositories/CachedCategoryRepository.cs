using System.Linq.Expressions;
using Domain.Models;
using Infrastructure.redis;

namespace Infrastructure.Repositories;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly IGenericRepository<Category> _repository;
    private readonly ICacheClient _cacheClient;

    public CachedCategoryRepository(IGenericRepository<Category> repository, ICacheClient client)
    {
        _repository = repository;
        _cacheClient = client;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        var key = id.ToString();
        return await _cacheClient.GetOrSetAndGetFromCache(key, () => _repository.GetByIdAsync(id));
    }

    public async Task AddAsync(Category entity)
    {
        await _repository.AddAsync(entity);
    }

    public async Task<List<Category>> SearchAsync(Expression<Func<Category, bool>> predicate)
    {
        return await _repository.SearchAsync(predicate);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var key = "category-key";
        var categories = await _cacheClient.GetOrSetAndGetFromCache(key, () => _repository.GetAllAsync());

        return categories ?? Enumerable.Empty<Category>();
    }


}