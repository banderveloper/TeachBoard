using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;
using TeachBoard.IdentityService.Persistence.EntityConfigurations;

namespace TeachBoard.IdentityService.Persistence;

// Main db context
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
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
    }
}