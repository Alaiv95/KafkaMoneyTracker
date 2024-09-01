using Domain.Entities.User;
using Infrastructure.Models;

namespace Infrastructure.Repositories.interfaces;

public interface IAuthRepository
{
    public Task AddUserAsync(UserEntity user);

    public Task<UserEntity?> GetByEmailAsync(string email);

    public Task<UserEntity?> GetByIdAsync(Guid id);
}
