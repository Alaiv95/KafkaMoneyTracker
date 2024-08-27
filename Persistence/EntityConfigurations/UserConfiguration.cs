using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(budget => budget.Id);
        builder.HasIndex(budget => budget.Id);
        builder.HasIndex(user => user.Email);

        builder.HasData(new User
        {
            Id = Guid.NewGuid(),
            Email = "Admin",
            PasswordHash = "AdminHash",
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        });
    }
}