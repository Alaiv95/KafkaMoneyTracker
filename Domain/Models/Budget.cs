namespace Domain.Models;

public class Budget : BaseEntity
{
    public double BudgetLimit { get; set; }

    public double? CurrentBudget { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public Category Category { get; set; }

    public User User { get; set; }

    public Guid CategoryId { get; set; }

    public Guid UserId { get; set; }
}