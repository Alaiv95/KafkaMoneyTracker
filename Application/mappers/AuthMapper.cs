using Application.Dtos;
using Application.handlers.auth.commands.Login;
using Application.handlers.auth.commands.Register;

namespace Application.mappers;

public class AuthMapper
{
    public LoginCommand DtoToLoginCommand(AuthDto dto)
    {
        return new LoginCommand
        {
            Email = dto.Email,
            Password = dto.Password,
        };
    }

    public RegisterUserCommand DtoToRegisterUserCommand(AuthDto dto)
    {
        return new RegisterUserCommand
        {
            Email = dto.Email,
            Password = dto.Password,
        };
    }
}
