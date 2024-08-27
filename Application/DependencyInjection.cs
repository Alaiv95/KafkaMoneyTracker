using Application.mappers;
using Application.specs;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<BudgetSpecs>();
        services.AddScoped<BudgetMapper>();

        return services;
    }
}
