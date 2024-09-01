using Infrastructure.EntityConfigurations;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class MoneyTrackerDbContext : DbContext, IMoneyTrackerDbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }

    public MoneyTrackerDbContext(DbContextOptions<MoneyTrackerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new CategoryConfiguration())
            .ApplyConfiguration(new TransactionConfiguration())
            .ApplyConfiguration(new BudgetConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}