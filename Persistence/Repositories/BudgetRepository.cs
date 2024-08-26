using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
{
    public BudgetRepository(IMoneyTrackerDbContext context) : base(context)
    {
    }

    public async Task<Budget?> GetActiveBudgetByUserAndCategory(Guid userId, Guid categoryId)
    {
        return await _dbSet.FirstOrDefaultAsync(budget =>
                 budget.UserId == userId &&
                 budget.CategoryId == categoryId &&
                 budget.PeriodEnd >= DateTime.Now
             );
    }

    public async Task<List<Budget>> SearchAsync(Expression<Func<Budget, bool>> predicate)
    {
        return await _dbSet.Where(p => p.Id == Guid.NewGuid()).ToListAsync();
    }
}
