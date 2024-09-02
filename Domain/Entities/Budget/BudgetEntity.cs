namespace Domain.Entities.Budget;

public class BudgetEntity : TimeBasedEntity
{
    private BudgetEntity() {}
    
    public Guid Id { get; private set; }
    
    public Limit BudgetLimit { get; private set; }

    public Guid CategoryId { get; private  set; }

    public Guid UserId { get; private set; }

    public static BudgetEntity Create(Limit limit, Guid categoryId, Guid userId)
    {
        if (categoryId == Guid.Empty || userId == Guid.Empty)
        {
            throw new ArgumentException("Some argument was invalid");
        }

        return new()
        {
            Id = Guid.NewGuid(),
            BudgetLimit = limit,
            CategoryId = categoryId,
            UserId = userId,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };
    }
}

public record Limit
{
    private Limit() { }
    
    public double Amount { get; private set; }

    public int Duration { get; private set; }

    public static Limit Create(double amount, int duration)
    {
        if (duration <= 0)
        {
            throw new ArgumentException("duration can't equals or be less then 0");
        }

        return new()
        {   
            Amount = amount,
            Duration = duration
        };
    }
}