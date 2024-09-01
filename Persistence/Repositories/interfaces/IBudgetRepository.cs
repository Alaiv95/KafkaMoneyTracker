using System.Linq.Expressions;
using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IBudgetRepository : IGenericRepository<Budget>
{
    Task<List<Budget>> SearchAsync(Expression<Func<Budget, bool>> predicate);

    Task UpdateOne(Budget budget);
}
