using System.Text.Json.Serialization;
using Domain.Entities.Budget;

namespace Domain.Entities.Transaction;

public class TransactionEntity : TimeBasedEntity
{
    [JsonConstructor]
    private TransactionEntity() {}
    
    public Guid Id { get; private set; }
    
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
            UpdatedAt = null,
            Id = Guid.NewGuid()
        };
    }
}

public record Money
{
    public double Amount { get; private set; }
    public string Currency { get; private set; }
    
    private Money() {}
    
    public static Money Create(double amount, string currency)
    {
        return new()
        {   
            Amount = amount,
            Currency = currency
        };
    }
}