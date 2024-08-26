using Application.budget.commands;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class BudgetCreateDto
{
    [Range(1.0, Double.MaxValue)]
    public Double BudgetLimit { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public Guid CategoryId { get; set; }

    public Guid UserId { get; set; }

    public CreateBudgetCommand ToCommand()
    {
        return new CreateBudgetCommand
        {
            BudgetLimit = BudgetLimit,
            PeriodStart = PeriodStart,
            PeriodEnd = PeriodEnd,
            CategoryId = CategoryId,
            UserId = UserId
        };
    }
}