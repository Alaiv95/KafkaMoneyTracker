using Domain.Models;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public interface IBudgetRepository : IGenericRepository<Budget>
{
    Task<Budget?> GetActiveBudgetByUserAndCategory(Guid userId, Guid categoryId);
    Task<List<Budget>> SearchAsync(Expression<Func<Budget, bool>> predicate);
}
