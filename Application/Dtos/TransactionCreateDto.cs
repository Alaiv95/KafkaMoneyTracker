using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class TransactionCreateDto
{
    [Required]
    public double Amount { get; set; }

    [Required]
    public Guid CategoryId { get; set; }
}
