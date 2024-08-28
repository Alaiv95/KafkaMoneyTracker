﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

public class MoneyTrackerDbContextFactory : IDesignTimeDbContextFactory<MoneyTrackerDbContext>
{
    public MoneyTrackerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MoneyTrackerDbContext>();
        optionsBuilder.UseSqlite("Data Source=Alaiv.KafkaMoneyTracker.db");

        return new MoneyTrackerDbContext(optionsBuilder.Options);
    }
}
