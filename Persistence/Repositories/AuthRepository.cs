using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    protected readonly IMoneyTrackerDbContext _context;

    public AuthRepository(IMoneyTrackerDbContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync(default);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        return user;
    }
}
