namespace Domain.Entities.Transaction;

public class TransactionEntity : TimeBasedEntity
{
    private TransactionEntity() {}
    
    public Guid UserId { get; private set; }

    public Money Money { get; private set; }
    
    public bool IsActive { get; private set; }

    public Guid CategoryId { get; private set; }

    public static TransactionEntity Create(Guid userId, Money money, Guid categoryId)
    {
        if (categoryId == Guid.Empty || userId == Guid.Empty)
        {
            throw new ArgumentException("Some argument was invalid");
        }

        return new()
        {
            Money = money,
            CategoryId = categoryId,
            IsActive = true,
            UserId = userId,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };
    }
}

public record Money(double Amount, string Currency);