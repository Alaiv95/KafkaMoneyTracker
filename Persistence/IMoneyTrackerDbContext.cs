using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure;

public interface IMoneyTrackerDbContext
{
    DbSet<Transaction> Transactions { get; set; }
    DbSet<Budget> Budgets { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken token);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}