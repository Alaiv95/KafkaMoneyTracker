﻿using Domain.Entities.User;
using Infrastructure.redis;
using Infrastructure.Repositories.interfaces;

namespace Infrastructure.Repositories;

public class CachedAuthRepository : IAuthRepository
{
    private readonly AuthRepository _authRepository;
    private readonly ICacheClient _cacheClient;

    public CachedAuthRepository(AuthRepository authRepository, ICacheClient cacheClient)
    {
        _authRepository = authRepository;
        _cacheClient = cacheClient;
    }

    public async Task AddUserAsync(UserEntity user)
    {
        await _authRepository.AddUserAsync(user);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        return await _cacheClient.GetOrSetAndGetFromCache(email, () => _authRepository.GetByEmailAsync(email));
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await _cacheClient.GetOrSetAndGetFromCache(id.ToString(), () => _authRepository.GetByIdAsync(id));
    }
}