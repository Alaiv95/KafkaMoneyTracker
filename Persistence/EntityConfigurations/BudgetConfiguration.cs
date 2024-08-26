using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(budget => budget.Id);
        builder.HasIndex(budget => budget.Id);

        builder
            .HasOne(budget => budget.User)
            .WithMany(user => user.Budgets)
            .HasForeignKey(budget => budget.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(budget => budget.Category)
            .WithMany(category => category.Budgets)
            .HasForeignKey(budget => budget.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}