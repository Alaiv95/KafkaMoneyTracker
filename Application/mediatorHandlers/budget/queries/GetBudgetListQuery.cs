using Application.mediator.interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.mediatorHandlers.budget.queries;

public class GetBudgetListQuery : IRequest
{
    [Required]
    public Guid UserId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public Guid CategoryId { get; set; }
}
