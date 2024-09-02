

using System.Linq.Expressions;
using Domain.Entities.Transaction;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;

namespace Infrastructure.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction, TransactionEntity>
{
    Task UpdateRangeAsync(List<Transaction> transactions);

    Task<List<Transaction>> GetByIdsAsync(List<Guid> itemIds);

    Task<List<Transaction>> SearchWithIncludeAsync(Expression<Func<Transaction, bool>> predicate);
}