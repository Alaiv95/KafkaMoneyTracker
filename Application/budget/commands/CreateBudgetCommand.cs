using System.ComponentModel.DataAnnotations;
using IRequest = Application.mediator.IRequest;

namespace Application.budget.commands;

public class CreateBudgetCommand : IRequest
{
    [Range(1.0, Double.MaxValue)]
    public Double BudgetLimit { get; set; }

    [Range (1, 365)]
    public int DurationInDays { get; set; }

    public Guid CategoryId { get; set; }

    public Guid UserId { get; set; }
}