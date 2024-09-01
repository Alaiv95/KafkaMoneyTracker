using Domain.Models;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public interface IBudgetRepository : IGenericRepository<Budget>
{
    Task<List<Budget>> SearchAsync(Expression<Func<Budget, bool>> predicate);
}
