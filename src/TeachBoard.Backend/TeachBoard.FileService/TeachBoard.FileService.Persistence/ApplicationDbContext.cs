using Microsoft.EntityFrameworkCore;
using TeachBoard.FileService.Application.Interfaces;
using TeachBoard.FileService.Domain.Entities;

namespace TeachBoard.FileService.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<CloudHomeworkSolutionFileInfo> HomeworkSolutions { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}