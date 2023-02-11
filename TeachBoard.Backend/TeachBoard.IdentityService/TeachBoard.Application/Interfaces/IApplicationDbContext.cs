using Microsoft.EntityFrameworkCore;
using TeachBoard.Domain.Entities;

namespace TeachBoard.Application.Interfaces;

// Interface for main db context
public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<RefreshSession> RefreshSessions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}