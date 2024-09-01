using System.Linq.Expressions;
using Domain.Models;
using Infrastructure.redis;

namespace Infrastructure.Repositories;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly CategoryRepository _repository;
    private readonly ICacheClient _cacheClient;
    private const string _getAllKey = "category-key";
    
    public CachedCategoryRepository(CategoryRepository categoryRepository, ICacheClient client)
    {
        _repository = categoryRepository;
        _cacheClient = client;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        var key = id.ToString();
        return await _cacheClient.GetOrSetAndGetFromCache(key, () => _repository.GetByIdAsync(id));
    }

    public async Task AddAsync(Category entity)
    {
        await _cacheClient.InvalidateCache(_getAllKey);
        await _repository.AddAsync(entity);
    }

    public async Task<List<Category>> SearchAsync(Expression<Func<Category, bool>> predicate)
    {
        return await _repository.SearchAsync(predicate);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var categories = await _cacheClient.GetOrSetAndGetFromCache(_getAllKey, () => _repository.GetAllAsync());

        return categories ?? Enumerable.Empty<Category>();
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _repository.GetByNameAsync(name);
    }

    public async Task DeleteAsync(Category category)
    {
        await _cacheClient.InvalidateCache(_getAllKey);
        await _cacheClient.InvalidateCache(category.Id.ToString());
        await _repository.DeleteAsync(category);
    }
}