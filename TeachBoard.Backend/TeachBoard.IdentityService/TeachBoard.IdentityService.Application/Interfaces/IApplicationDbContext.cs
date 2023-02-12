using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.Interfaces;

// Interface for main db context
public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<RefreshSession> RefreshSessions { get; set; }
    DbSet<PendingUser> PendingUsers { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}