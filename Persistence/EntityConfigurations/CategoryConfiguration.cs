using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(budget => budget.Id);
        builder.HasIndex(budget => budget.Id);


        builder.HasData(
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryType = CategoryType.Entertainment,
            },
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryType = CategoryType.Food,
            },
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryType = CategoryType.Salary,
            },
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryType = CategoryType.Transport,
            }
        );
    }
}