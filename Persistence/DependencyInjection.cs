using Infrastructure.redis;
using Infrastructure.Repositories;
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
        collection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        collection.AddScoped<IBudgetRepository, BudgetRepository>();
        collection.AddScoped<AuthRepository>();
        collection.AddScoped<IAuthRepository, AuthCachedRepository>();
        collection.AddScoped<ITransactionRepository, TransactionRepository>();
        collection.AddScoped<ICategoryRepository, CachedCategoryRepository>();
        collection.AddScoped<ICacheClient, RedisCacheClient>();

        return collection;
    }
}