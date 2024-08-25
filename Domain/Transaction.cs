namespace Domain;

public class Transaction : BaseEntity
{
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    public Double Amount { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public Category Category { get; set; }
}