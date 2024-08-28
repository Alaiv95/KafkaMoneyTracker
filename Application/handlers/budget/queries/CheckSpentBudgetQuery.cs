using Application.mediator.interfaces;

namespace Application.handlers.budget.queries;

public class CheckSpentBudgetQuery : IRequest<bool>
{
    public Guid UserId { get; set; }

    public Guid CategoryId { get; set; }
}
