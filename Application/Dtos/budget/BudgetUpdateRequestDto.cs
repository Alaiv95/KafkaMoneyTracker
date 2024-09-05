using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos;

public class BudgetUpdateRequestDto
{
    [Range(1.0, double.MaxValue)]
    public double? BudgetLimit { get; set; }
    
    [Range(1, 365)]
    public int? DurationInDays { get; set; }
    
    [Required]
    public Guid CategoryId { get; set; }
}