using System.Linq.Expressions;
using Domain.Entities.Transaction;
using Infrastructure.Models;

namespace Infrastructure.Repositories.interfaces;

public interface ITransactionRepository : IGenericRepository<Transaction, TransactionEntity>
{
    Task UpdateRangeAsync(List<Transaction> transactions);

    Task<List<Transaction>> GetByIdsAsync(List<Guid> itemIds);

    Task<List<Transaction>> SearchWithIncludeAsync(Expression<Func<Transaction, bool>> predicate);
}