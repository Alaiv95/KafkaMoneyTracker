using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
{
    public BudgetRepository(IMoneyTrackerDbContext context) : base(context)
    {
    }

    public async Task<List<Budget>> SearchAsync(Expression<Func<Budget, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
}
