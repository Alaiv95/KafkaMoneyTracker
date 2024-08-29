using Application.exceptions;
using Application.kafka.producer;
using Application.mediator.interfaces;
using Domain.Models;
using Infrastructure.authentication;
using Infrastructure.Repositories;

namespace Application.handlers.auth.commands.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEventsProducer _eventsProducer;

    public RegisterUserCommandHandler(IAuthRepository authRepository, IPasswordHasher passwordHasher, IEventsProducer eventsProducer)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _eventsProducer = eventsProducer;
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
            Id = Guid.NewGuid(),
            Email = command.Email,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
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
