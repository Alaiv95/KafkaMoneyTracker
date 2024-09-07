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
        Expression<Func<Transaction, bool>> predicate
    )
    {
        var transactions = await _dbSet
            .AsNoTracking()
            .Where(predicate)
            .Include(t => t.Budget)
            .ThenInclude(b => b.Category)
            .ToListAsync();
        
        return Mapper.Map<List<TransactionInfo>>(transactions);
    }

    public async Task<List<TransactionInfo>> SearchWithIncludeAsync(
        Expression<Func<Transaction, bool>> predicate,
        int page,
        int limit
    )
    {
        var transactions = await _dbSet
            .AsNoTracking()
            .Where(predicate)
            .Skip(page)
            .Take(limit)
            .OrderBy(t => t.Amount)
            .Include(t => t.Budget)
            .ThenInclude(b => b.Category)
            .ToListAsync();

        return Mapper.Map<List<TransactionInfo>>(transactions);
    }
}