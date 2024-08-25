namespace Domain;

public class User : BaseEntity
{
    public String Login { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
    
    public ICollection<Budget> Budgets { get; set; }
}