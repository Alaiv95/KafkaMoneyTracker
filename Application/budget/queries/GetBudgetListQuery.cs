using Application.mediator.interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.budget.queries;

public class GetBudgetListQuery : IRequest
{
    [Required]
    public Guid UserId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }
}
