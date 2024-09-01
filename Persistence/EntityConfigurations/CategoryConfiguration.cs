using Domain.Enums;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(budget => budget.Id);
        builder.HasIndex(budget => budget.Id).IsUnique();

        builder.HasData(
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryType = CategoryType.Entertainment,
                CategoryName = CategoryType.Entertainment.ToString(),
                IsCustom = false
            },
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryType = CategoryType.Food,
                CategoryName = CategoryType.Food.ToString(),
                IsCustom = false
            },
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryType = CategoryType.Salary,
                CategoryName = CategoryType.Salary.ToString(),
                IsCustom = false
            },
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryType = CategoryType.Transport,
                CategoryName = CategoryType.Transport.ToString(),
                IsCustom = false
            }
        );
    }
}