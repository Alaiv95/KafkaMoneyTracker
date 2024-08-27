using Infrastructure.authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) {

        services.AddScoped<ITokenFactory, JwtTokenFactory>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
