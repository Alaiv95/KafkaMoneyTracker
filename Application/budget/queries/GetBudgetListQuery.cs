using Application.mediator;

namespace Application.budget.queries;

public class GetBudgetListQuery : IRequest
{
    public Guid UserId { get; set; }

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }
}
