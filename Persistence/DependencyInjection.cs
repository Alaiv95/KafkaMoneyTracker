using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection collection, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("DefaultConnection");

        collection.AddDbContext<MoneyTrackerDbContext>(options => options.UseSqlite(dbConnection));
        collection.AddScoped<IMoneyTrackerDbContext>(provider => provider.GetRequiredService<MoneyTrackerDbContext>());

        return collection;
    }
}