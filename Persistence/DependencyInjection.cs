using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection collection, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("DefaultConnection");

        collection.AddDbContext<MoneyTrackerDbContext>(options => options.UseSqlite(dbConnection));
        collection.AddScoped<IMoneyTrackerDbContext>(provider => provider.GetRequiredService<MoneyTrackerDbContext>());
        collection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        collection.AddScoped<IBudgetRepository, BudgetRepository>();
        collection.AddScoped<IAuthRepository, AuthRepository>();

        return collection;
    }
}