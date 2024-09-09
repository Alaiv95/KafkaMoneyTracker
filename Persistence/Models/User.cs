namespace Infrastructure.Models;

public class User : BaseModel
{
    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public ICollection<Transaction> Transactions { get; set; }

    public ICollection<Budget> Budgets { get; set; }
    
    public string MainCurrency { get; set; }
}