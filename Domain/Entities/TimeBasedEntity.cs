namespace Domain.Entities;

public class TimeBasedEntity
{
    public DateTime CreatedAt { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }
}