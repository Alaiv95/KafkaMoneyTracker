using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;
public class AuthDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(4)]
    public string Password { get; set; }
    
    [MaxLength(3)]
    [MinLength(3)]
    public string MainCurrency { get; set; }
}
