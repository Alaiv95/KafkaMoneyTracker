namespace Domain.Models;

public class User : BaseEntity
{
    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public ICollection<Transaction> Transactions { get; set; }

    public ICollection<Budget> Budgets { get; set; }
}