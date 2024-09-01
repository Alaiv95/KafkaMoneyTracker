using Application.Dtos;
using Application.handlers.auth.commands.Login;
using Application.handlers.auth.commands.Register;
using AutoMapper;

namespace Application.mappers;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<AuthDto, LoginCommand>();
        CreateMap<AuthDto, RegisterUserCommand>();
    }
}