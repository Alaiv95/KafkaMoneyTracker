using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
{
    public BudgetRepository(IMoneyTrackerDbContext context) : base(context)
    {
    }

    public async Task UpdateOne(Budget budget)
    {
        _dbSet.Update(budget);
        await _context.SaveChangesAsync(default);
    }
}
