
namespace Infrastructure.authentication;

public interface ITokenFactory
{
    string Generate(Guid userId, string userEmail);
}