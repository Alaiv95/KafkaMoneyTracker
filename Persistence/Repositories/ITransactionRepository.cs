

using Domain.Models;

namespace Infrastructure.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task UpdateRangeAsync(List<Transaction> transactions);

    Task<List<Transaction>> GetByIdsAsync(List<Guid> itemIds);
}