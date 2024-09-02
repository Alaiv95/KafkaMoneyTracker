using Application.mediator.interfaces;

namespace Application.handlers.budget.queries.CheckSpentBudget;

public class CheckSpentBudgetQuery : IRequest<bool>
{
    public Guid UserId { get; set; }

    public Guid BudgetId { get; set; }
}
