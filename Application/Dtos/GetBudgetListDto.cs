using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class GetBudgetListDto
{
    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public Guid CategoryId { get; set; }
}
