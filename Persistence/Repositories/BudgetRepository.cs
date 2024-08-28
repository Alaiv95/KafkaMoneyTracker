using Domain.Models;

namespace Infrastructure.Repositories;

public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
{
    public BudgetRepository(IMoneyTrackerDbContext context) : base(context)
    {
    }

    // TODO add update budget
}
