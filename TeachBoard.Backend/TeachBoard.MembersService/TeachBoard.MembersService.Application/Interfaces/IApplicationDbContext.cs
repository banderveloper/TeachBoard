using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Interfaces;

/// <summary>
/// Interface of main db-context
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Student> Students { get; set; }
    DbSet<Teacher> Teachers { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<Feedback> Feedbacks { get; set; }
    DbSet<NewsItem> NewsItems { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}