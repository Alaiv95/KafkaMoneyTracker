

using Domain.Models;

namespace Infrastructure.Repositories;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    public Task<List<Transaction>> DeactivateTransactionsAsync(Dictionary<Guid, Guid> transactionIds);
}