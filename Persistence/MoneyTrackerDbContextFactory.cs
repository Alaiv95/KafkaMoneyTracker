using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class MoneyTrackerDbContextFactory : IDesignTimeDbContextFactory<MoneyTrackerDbContext>
{
    public MoneyTrackerDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        var dbConnection = configuration.GetConnectionString("DefaultConnection");
        
        var optionsBuilder = new DbContextOptionsBuilder<MoneyTrackerDbContext>();
        optionsBuilder.UseNpgsql(dbConnection);

        return new MoneyTrackerDbContext(optionsBuilder.Options);
    }
    
    private IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }
}
