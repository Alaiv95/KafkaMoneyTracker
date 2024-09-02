using Domain.Entities.Budget;
using Infrastructure.Models;

namespace Infrastructure.Repositories.interfaces;

public interface IBudgetRepository : IGenericRepository<Budget, BudgetEntity>
{
    Task UpdateOne(Guid id, Limit limit);
}