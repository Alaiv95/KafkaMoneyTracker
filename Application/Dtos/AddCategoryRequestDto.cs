using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class AddCategoryRequestDto
{
    [MinLength(4)]
    [MaxLength(50)]
    [Required]
    public string Name { get; set; }
}