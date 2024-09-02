using AutoMapper;
using Domain.Entities.User;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IMoneyTrackerDbContext _context;
    private readonly IMapper _mapper;

    public AuthRepository(IMoneyTrackerDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddUserAsync(UserEntity userEntity)
    {
        var user = _mapper.Map<User>(userEntity);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync(default);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        var user = await GetUserByEmailAsync(email);

        return _mapper.Map<UserEntity>(user);
    }

    internal async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        var user = await GetUserByIdAsync(id);

        return _mapper.Map<UserEntity>(user);
    }
    
    internal async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}
