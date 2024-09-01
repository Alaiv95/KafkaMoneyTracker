using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.User;

public class UserEntity : TimeBasedEntity
{
    private UserEntity() { }
    
    public Guid Id { get; private set; }
    
    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public static UserEntity Create(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentException($"email {email} or password {password} are invalid");
        }

        if (password.Length < 4)
        {
            throw new ArgumentException($"Password length can't be less then 4 symbols");
        }
        
        return new()
        {
            Id = Guid.NewGuid(),
            PasswordHash = password,
            Email = email,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };
    }
}