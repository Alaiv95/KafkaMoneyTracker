using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository 
{
    public TransactionRepository(IMoneyTrackerDbContext context) 
        : base(context) { }

    public async Task UpdateRangeAsync(List<Transaction> transactions)
    {
        _dbSet.UpdateRange(transactions);
        await _context.SaveChangesAsync(default);
    }

    public async Task<List<Transaction>> GetByIdsAsync(List<Guid> itemIds)
    {
        return await _dbSet.Where(t => itemIds.Contains(t.Id)).ToListAsync();
    }
}

