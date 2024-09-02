namespace Application.Dtos;

public class BaseFilterSearchDto
{
    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public Guid? BudgetId { get; set; }
}
