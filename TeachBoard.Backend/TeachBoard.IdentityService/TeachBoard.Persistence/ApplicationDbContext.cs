using Microsoft.EntityFrameworkCore;
using TeachBoard.Application.Interfaces;
using TeachBoard.Domain.Entities;
using TeachBoard.Persistence.EntityConfigurations;

namespace TeachBoard.Persistence;

// Main db context
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshSession> RefreshSessions { get; set; }
    public DbSet<PendingUser> PendingUsers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // apply custom fluent api configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
}