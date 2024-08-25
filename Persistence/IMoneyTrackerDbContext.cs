using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public interface IMoneyTrackerDbContext
{
    DbSet<Transaction> Transactions { get; set; }
    DbSet<Budget> Budgets { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<User> Users { get; set; }
}