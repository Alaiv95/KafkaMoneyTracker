using AutoMapper;
using Domain.Entities.Budget;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BudgetRepository : GenericRepository<Budget, BudgetEntity>, IBudgetRepository
{
    public BudgetRepository(IMoneyTrackerDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task UpdateOne(Guid id, Limit limit)
    {
        await _dbSet
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(e => e
                .SetProperty(x => x.DurationInDays, limit.Duration)
                .SetProperty(x => x.BudgetLimit, limit.Amount)
            );
        
        await _context.SaveChangesAsync(default);
    }
}