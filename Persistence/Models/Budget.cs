namespace Infrastructure.Models;

public class Budget : BaseModel
{
    public double BudgetLimit { get; set; }

    public int DurationInDays { get; set; }

    public Category Category { get; set; }

    public User User { get; set; }

    public Guid CategoryId { get; set; }

    public Guid UserId { get; set; }
}