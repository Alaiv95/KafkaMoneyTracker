using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class BudgetCreateDto
{
    [Range(1.0, double.MaxValue)]
    [Required]
    public double BudgetLimit { get; set; }

    [Range(1, 365)]
    [Required]
    public int DurationInDays { get; set; }

    [Required]
    public Guid CategoryId { get; set; }
}