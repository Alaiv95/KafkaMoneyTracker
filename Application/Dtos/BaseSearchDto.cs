namespace Application.Dtos;

public class BaseSearchDto
{
    public Guid UserId { get; set; }

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public Guid? BudgetId { get; set; }
}
