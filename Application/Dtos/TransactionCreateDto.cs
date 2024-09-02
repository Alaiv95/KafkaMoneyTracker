using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class TransactionCreateDto
{
    [Required]
    public double Amount { get; set; }
    
    [StringLength(3)]
    [Required]
    public string Currency { get; set; }
    
    [Required]
    public Guid BudgetId { get; set; }
}
