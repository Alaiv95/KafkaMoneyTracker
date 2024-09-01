using Application.exceptions;
using Application.mediator.interfaces;
using Domain.Entities.User;
using Infrastructure.authentication;
using Infrastructure.Repositories.interfaces;

namespace Application.handlers.auth.commands.Register;

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
        var user = UserEntity.Create(command.Email, hashedPassword);

        await _authRepository.AddUserAsync(user);

        return true;
    }

    private async Task<bool> IsUserAlreadyRegistered(string email)
    {
        var user = await _authRepository.GetByEmailAsync(email);

        return user != null;
    }
}