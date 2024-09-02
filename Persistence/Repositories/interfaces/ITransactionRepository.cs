using System.Linq.Expressions;
using Domain.Entities.Transaction;
using Infrastructure.Models;

namespace Infrastructure.Repositories.interfaces;

public interface ITransactionRepository : IGenericRepository<Transaction, TransactionEntity>
{
    Task UpdateRangeAsync(List<TransactionEntity> transactions);

    Task<List<TransactionEntity>> GetByIdsAsync(List<Guid> itemIds);

    Task<List<TransactionInfo>> SearchWithIncludeAsync(Expression<Func<Transaction, bool>> predicate);
}