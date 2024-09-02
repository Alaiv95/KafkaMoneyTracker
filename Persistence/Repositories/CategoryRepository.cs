using AutoMapper;
using Domain.Entities;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : GenericRepository<Category, CategoryEntity>, ICategoryRepository
{
    public CategoryRepository(IMoneyTrackerDbContext context, IMapper mapper) 
        : base(context, mapper)
    {
    }

    public async Task<CategoryEntity?> GetByNameAsync(string name)
    {
        var model = await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryName == name);

        return Mapper.Map<CategoryEntity>(model);
    }

    public async Task DeleteAsync(CategoryEntity category)
    {
        await _dbSet.Where(c => c.Id == category.Id).ExecuteDeleteAsync();
        await _context.SaveChangesAsync(default);
    }

    internal async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _dbSet.ToListAsync();
    }
}