using Infrastructure.redis;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection collection, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        var dbConnection = connectionString ?? configuration.GetConnectionString("DefaultConnection");

        collection.AddDbContext<MoneyTrackerDbContext>(options => options.UseNpgsql(dbConnection));
        collection.AddScoped<IMoneyTrackerDbContext>(provider => provider.GetRequiredService<MoneyTrackerDbContext>());

        collection.AddScoped<BudgetSpecs>();
        collection.AddScoped<TransactionSpecs>();

        collection.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        collection.AddScoped<IBudgetRepository, BudgetRepository>();
        collection.AddScoped<AuthRepository>();
        collection.AddScoped<IAuthRepository, CachedAuthRepository>();
        collection.AddScoped<ITransactionRepository, TransactionRepository>();
        collection.AddScoped<CategoryRepository>();
        collection.AddScoped<ICategoryRepository, CachedCategoryRepository>();
        collection.AddScoped<IExchangeRatesRepository, ExchangeRatesRepository>();

        collection.AddScoped<ICacheClient, RedisCacheClient>();

        return collection;
    }
}