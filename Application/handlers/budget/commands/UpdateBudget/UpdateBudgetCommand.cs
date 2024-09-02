using System.ComponentModel.DataAnnotations;
using Application.Dtos;
using Application.mediator.interfaces;
using Domain.Entities.Budget;

namespace Application.handlers.budget.commands.UpdateBudget;

public class UpdateBudgetCommand : IRequest<Limit>
{
    [Range(1.0, double.MaxValue)]
    public double? BudgetLimit { get; set; }
    
    [Range(1, 365)]
    public int? DurationInDays { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public Guid UserId { get; set; }
}