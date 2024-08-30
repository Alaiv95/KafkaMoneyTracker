using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository 
{
    public TransactionRepository(IMoneyTrackerDbContext context) 
        : base(context) { }

    public async Task<List<Transaction>> DeactivateTransactionsAsync(Dictionary<Guid, Guid> transactionIds)
    {
        var transactions = await _dbSet
            .Where(t => transactionIds.ContainsKey(t.Id))
            .ToListAsync();
        
        transactions.ForEach(t => t.IsActive = false);
        
        _dbSet.UpdateRange(transactions);
        await _context.SaveChangesAsync(default);

        return transactions;
    }
}