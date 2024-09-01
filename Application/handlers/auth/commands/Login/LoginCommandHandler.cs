using Application.mediator.interfaces;
using Infrastructure.Repositories;
using Application.exceptions;
using Infrastructure.authentication;
using Infrastructure.Repositories.interfaces;

namespace Application.handlers.auth.commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenFactory _tokenFactory;

    public LoginCommandHandler(IAuthRepository authRepository, IPasswordHasher passwordHasher, ITokenFactory tokenFactory)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _tokenFactory = tokenFactory;
    }

    public async Task<string> Handle(LoginCommand command)
    {
        var user = await _authRepository.GetByEmailAsync(command.Email);
        var isPasswordValid = _passwordHasher.Verify(command.Password, user?.PasswordHash);

        if (user == null || !isPasswordValid)
        {
            throw new ForbiddenException("Email or Password are incorrect");
        }

        return _tokenFactory.Generate(user.Id, user.Email);
    }
}
