using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class BudgetCreateDto
{
    [Range(1.0, double.MaxValue)]
    public double BudgetLimit { get; set; }

    [Range(1, 365)]
    public int DurationInDays { get; set; }

    public Guid CategoryId { get; set; }

    public Guid UserId { get; set; }
}