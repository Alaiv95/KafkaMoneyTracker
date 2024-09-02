namespace Infrastructure.Models;

public class Transaction : BaseModel
{
    public Guid UserId { get; set; }

    public User User { get; set; }

    public double Amount { get; set; }

    public string Currency { get; set; } = "RUB";
    
    public bool IsActive { get; set; }

    public Guid BudgetId { get; set; }

    public Budget Budget { get; set; }
}