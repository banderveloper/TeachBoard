using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeachBoard.Domain.Entities;

namespace TeachBoard.Persistence.EntityConfigurations;

// Fluent api configuration for entity User
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(i => i.UserName).IsUnique();
    }
}