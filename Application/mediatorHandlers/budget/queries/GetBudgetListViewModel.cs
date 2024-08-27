namespace Application.mediatorHandlers.budget.queries;

public class BudgetLookUpDto
{
    public double BudgetLimit { get; set; }

    public int DurationInDays { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CategoryId { get; set; }
}
