using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Persistence;

/// <summary>
/// Entity framework database context
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<StudentToTeacherFeedback> StudentToTeacherFeedbacks { get; set; }
    public DbSet<TeacherToStudentFeedback> TeacherToStudentFeedbacks { get; set; }
    public DbSet<NewsItem> NewsItems { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">Options set at DI for switching provider, connection string, ...</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    
}