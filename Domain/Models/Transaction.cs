namespace Domain.Models;

public class Transaction : BaseEntity
{
    public Guid UserId { get; set; }

    public User User { get; set; }

    public double Amount { get; set; }

    public Guid CategoryId { get; set; }

    public Category Category { get; set; }
}