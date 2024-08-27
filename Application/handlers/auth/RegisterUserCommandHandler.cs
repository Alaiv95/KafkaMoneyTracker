using Application.exceptions;
using Application.mediator.interfaces;
using Domain.Models;
using Infrastructure.authentication;
using Infrastructure.Repositories;

namespace Application.mediatorHandlers.auth;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IAuthRepository authRepository, IPasswordHasher passwordHasher)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(RegisterUserCommand command)
    {
        var userIsRegistered = await IsUserAlreadyRegistered(command.Email);

        if (userIsRegistered)
        {
            throw new UserAlreadyExistsException($"Email {command.Email} already exists");
        }

        var hashedPassword = _passwordHasher.Generate(command.Password);

        var user = new User
        {
            Email = command.Email,
            PasswordHash = hashedPassword,
        };

        await _authRepository.AddUserAsync(user);

        return true;
    }

    private async Task<bool> IsUserAlreadyRegistered(string email)
    {
        var user = await _authRepository.GetByEmailAsync(email);

        return user != null;
    }
}
