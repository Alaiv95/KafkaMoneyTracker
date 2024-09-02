using System.Linq.Expressions;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Models;
using Infrastructure.redis;
using Infrastructure.Repositories.interfaces;

namespace Infrastructure.Repositories;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly CategoryRepository _repository;
    private readonly ICacheClient _cacheClient;
    private readonly IMapper _mapper;
    private const string _getAllKey = "category-key";
    
    public CachedCategoryRepository(CategoryRepository categoryRepository, ICacheClient client, IMapper mapper)
    {
        _repository = categoryRepository;
        _cacheClient = client;
        _mapper = mapper;
    }

    public async Task<CategoryEntity?> GetByIdAsync(Guid id)
    {
        var key = id.ToString();
        return await _cacheClient.GetOrSetAndGetFromCache(key, () => _repository.GetByIdAsync(id));
    }

    public async Task AddAsync(CategoryEntity entity)
    {
        await _cacheClient.InvalidateCache(_getAllKey);
        await _repository.AddAsync(entity);
    }

    public async Task<List<CategoryEntity>> SearchAsync(Expression<Func<Category, bool>> predicate)
    {
        return await _repository.SearchAsync(predicate);
    }

    public async Task<List<CategoryEntity>> GetAllAsync()
    {
        var categories = await _cacheClient.GetOrSetAndGetFromCache(_getAllKey, () => _repository.GetAllCategoriesAsync());
        var categoryEntities = _mapper.Map<List<CategoryEntity>>(categories);
        
        return categoryEntities ?? new List<CategoryEntity>();
    }

    public async Task<CategoryEntity?> GetByNameAsync(string name)
    {
        return await _repository.GetByNameAsync(name);
    }

    public async Task DeleteAsync(CategoryEntity category)
    {
        await _cacheClient.InvalidateCache(_getAllKey);
        await _cacheClient.InvalidateCache(category.Id.ToString());
        await _repository.DeleteAsync(category);
    }
}