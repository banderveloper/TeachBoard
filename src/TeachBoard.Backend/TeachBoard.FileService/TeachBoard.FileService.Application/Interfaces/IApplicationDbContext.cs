using Microsoft.EntityFrameworkCore;
using TeachBoard.FileService.Domain.Entities;

namespace TeachBoard.FileService.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CloudHomeworkSolutionFileInfo> HomeworkSolutions { get; set; }
    DbSet<CloudHomeworkTaskFileInfo> HomeworkTasks { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}