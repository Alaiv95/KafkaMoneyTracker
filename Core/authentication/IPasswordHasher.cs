namespace Infrastructure.authentication;

public interface IPasswordHasher
{
    string Generate(string password);
    bool Verify(string password, string? hashpassword);
}