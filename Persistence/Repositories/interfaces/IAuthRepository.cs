using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IAuthRepository
{
    public Task AddUserAsync(User user);

    public Task<User?> GetByEmailAsync(string email);

    public Task<User?> GetByIdAsync(Guid id);
}
