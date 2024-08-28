namespace Application.Dtos;

public class BudgetGetListDto
{
    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public Guid? CategoryId { get; set; }
}
