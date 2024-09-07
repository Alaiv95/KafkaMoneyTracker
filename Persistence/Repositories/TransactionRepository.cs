using System.Linq.Expressions;
using AutoMapper;
using Domain.Entities.Transaction;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository : GenericRepository<Transaction, TransactionEntity>, ITransactionRepository
{
    public TransactionRepository(IMoneyTrackerDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public async Task UpdateRangeAsync(List<TransactionEntity> transactions)
    {
        var transactionsList = Mapper.Map<List<Transaction>>(transactions);
        _dbSet.UpdateRange(transactionsList);
        await _context.SaveChangesAsync(default);
    }

    public async Task<List<TransactionEntity>> GetByIdsAsync(List<Guid> itemIds)
    {
        var transactions = await _dbSet
            .AsNoTracking()
            .Where(t => itemIds.Contains(t.Id))
            .ToListAsync();

        return Mapper.Map<List<TransactionEntity>>(transactions);
    }

    public async Task<List<TransactionInfo>> SearchWithIncludeAsync(
        Expression<Func<Transaction, bool>> predicate,
        int? page = null,
        int? limit = null
    )
    {
        var query = _dbSet
            .AsNoTracking()
            .Where(predicate);

        if (page.HasValue && limit.HasValue)
        {
            var skipAmount = (page.Value - 1) * limit.Value;
            query = query.Skip(skipAmount).Take(limit.Value);
        }
        
        query = query
            .OrderBy(t => t.Amount)
            .Include(t => t.Budget)
            .ThenInclude(b => b.Category);

        var transactions =  await query.ToListAsync();
        return Mapper.Map<List<TransactionInfo>>(transactions);
    }

    public async Task<int> CountTransactionsAsync(Expression<Func<Transaction, bool>> predicate)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(predicate)
            .CountAsync();
    }
}