using Domain.Enums;

namespace Application.handlers.budget.queries.models;

public class BudgetExceededDto
{
    public string Category { get; set; }

    public double SpentAmount { get; set; }

    public string BudgetPeriod { get; set; }

    public double BudgetLimit { get; set; }

    public Guid UserId { get; set; }
}
