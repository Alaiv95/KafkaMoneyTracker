using Application.Dtos;
using Application.handlers.auth.commands.Login;
using Application.handlers.auth.commands.Register;
using AutoMapper;
using Domain.Entities.User;
using Infrastructure.Models;

namespace Application.mappers;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<AuthDto, LoginCommand>();
        CreateMap<AuthDto, RegisterUserCommand>();

        CreateMap<UserEntity, User>();
        CreateMap<User, UserEntity>();
    }
}