namespace Core.common;

public class BaseSearchDto
{
    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public Guid? BudgetId { get; set; }
}
