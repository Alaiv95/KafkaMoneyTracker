using Core.mail_client;
using Infrastructure.authentication;
using Infrastructure.FileUtils.writers;
using Infrastructure.MailClients;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ITokenFactory, JwtTokenFactory>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IMailClient, SmtpMailClient>();
        services.AddSingleton<FileWriterFactory>();

        return services;
    }
}