using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IMoneyTrackerDbContext context) 
        : base(context)
    {
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryName == name);
    }

    public async Task DeleteAsync(Category category)
    {
        _dbSet.Remove(category);
        await _context.SaveChangesAsync(default);
    }
}