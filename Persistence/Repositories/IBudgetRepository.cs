using Domain;

namespace Infrastructure.Repositories;

public interface IBudgetRepository : IGenericRepository<Budget>
{
    Task<Budget?> GetActiveBudgetByUserAndCategory(Guid userId, Guid categoryId);
}
