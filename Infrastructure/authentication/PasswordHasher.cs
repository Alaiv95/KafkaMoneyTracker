namespace Infrastructure.authentication;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string? hashpassword)
    {
        if (hashpassword == null)
        {
            return false;
        }

        return BCrypt.Net.BCrypt.Verify(password, hashpassword);
    }
}
