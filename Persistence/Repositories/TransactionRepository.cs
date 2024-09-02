using System.Linq.Expressions;
using AutoMapper;
using Domain.Entities.Transaction;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository : GenericRepository<Transaction, TransactionEntity>, ITransactionRepository
{
    public TransactionRepository(IMoneyTrackerDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public async Task UpdateRangeAsync(List<Transaction> transactions)
    {
        _dbSet.UpdateRange(transactions);
        await _context.SaveChangesAsync(default);
    }

    public async Task<List<Transaction>> GetByIdsAsync(List<Guid> itemIds)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(t => itemIds.Contains(t.Id))
            .ToListAsync();
    }

    public async Task<List<Transaction>> SearchWithIncludeAsync(Expression<Func<Transaction, bool>> predicate)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(predicate)
            .Include(t => t.Category)
            .ToListAsync();
    }
}