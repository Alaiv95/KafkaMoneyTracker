namespace Core.common;

public class BaseBudgetSearchFilter
{
    public Guid UserId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public Guid? BudgetId { get; set; }
}
