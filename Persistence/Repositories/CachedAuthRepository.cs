using AutoMapper;
using Domain.Entities.User;
using Infrastructure.redis;
using Infrastructure.Repositories.interfaces;

namespace Infrastructure.Repositories;

public class CachedAuthRepository : IAuthRepository
{
    private readonly AuthRepository _authRepository;
    private readonly ICacheClient _cacheClient;
    private readonly IMapper _mapper;

    public CachedAuthRepository(AuthRepository authRepository, ICacheClient cacheClient, IMapper mapper)
    {
        _authRepository = authRepository;
        _cacheClient = cacheClient;
        _mapper = mapper;
    }

    public async Task AddUserAsync(UserEntity user)
    {
        await _authRepository.AddUserAsync(user);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        var user = await _cacheClient.GetOrSetAndGetFromCache(email, () => _authRepository.GetUserByEmailAsync(email));
        
        return _mapper.Map<UserEntity>(user);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        var user = await _cacheClient.GetOrSetAndGetFromCache(id.ToString(), () => _authRepository.GetUserByIdAsync(id));
        
        return _mapper.Map<UserEntity>(user);
    }
}