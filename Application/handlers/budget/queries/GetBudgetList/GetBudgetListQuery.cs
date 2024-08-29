using Application.mediator.interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.handlers.budget.queries.GetBudgetList;

public class GetBudgetListQuery : IRequest<BudgetListVm>
{
    [Required]
    public Guid UserId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public Guid? CategoryId { get; set; }
}
