using Application.Dtos;
using Application.mediatorHandlers.auth;

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
