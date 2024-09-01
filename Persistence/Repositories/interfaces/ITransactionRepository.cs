

using System.Linq.Expressions;
using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task UpdateRangeAsync(List<Transaction> transactions);

    Task<List<Transaction>> GetByIdsAsync(List<Guid> itemIds);

    Task<List<Transaction>> SearchWithIncludeAsync(Expression<Func<Transaction, bool>> predicate);
}