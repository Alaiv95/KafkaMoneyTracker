using System.Text.Json.Serialization;

namespace Domain.Entities.Transaction;

public class TransactionEntity : TimeBasedEntity
{
    [JsonConstructor]
    private TransactionEntity()
    {
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Money Money { get; private set; }

    public bool IsActive { get; private set; }

    public Guid BudgetId { get; private set; }

    public static TransactionEntity Create(Guid userId, Money money, Guid budgetId)
    {
        if (budgetId == Guid.Empty || userId == Guid.Empty)
        {
            throw new ArgumentException($"UserId {userId} or {budgetId} was invalid");
        }

        return new()
        {
            Money = money,
            BudgetId = budgetId,
            IsActive = true,
            UserId = userId,
            CreatedAt = DateTime.Now,
            UpdatedAt = null,
            Id = Guid.NewGuid()
        };
    }

    public void Deactivate() => IsActive = false;
}

public record Money
{
    public double Amount { get; private set; }
    public string Currency { get; private set; }

    private Money()
    {
    }

    public static Money Create(double amount, string currency)
    {
        if (currency.Length != 3)
        {
            throw new ArgumentException("Currency should contain 3 characters.");
        }

        return new()
        {
            Amount = amount,
            Currency = currency
        };
    }
}