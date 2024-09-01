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
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        return _mapper.Map<UserEntity>(user);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<UserEntity>(user);
    }
}
