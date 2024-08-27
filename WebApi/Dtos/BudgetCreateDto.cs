using Application.budget.commands;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class BudgetCreateDto
{
    [Range(1.0, Double.MaxValue)]
    public Double BudgetLimit { get; set; }

    [Range(1, 365)]
    public int DurationInDays { get; set; }

    public Guid CategoryId { get; set; }

    public Guid UserId { get; set; }

    public CreateBudgetCommand ToCommand()
    {
        return new CreateBudgetCommand
        {
            BudgetLimit = BudgetLimit,
            DurationInDays = DurationInDays,
            CategoryId = CategoryId,
            UserId = UserId
        };
    }
}