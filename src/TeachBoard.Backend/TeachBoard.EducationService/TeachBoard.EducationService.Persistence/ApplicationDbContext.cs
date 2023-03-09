using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Entities;

namespace TeachBoard.EducationService.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<CompletedHomework> CompletedHomeworks { get; set; }
    public DbSet<Examination> Examinations { get; set; }
    public DbSet<Homework> Homeworks { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<StudentExaminationActivity> StudentExaminationActivities { get; set; }
    public DbSet<StudentLessonActivity> StudentLessonActivities { get; set; }
    public DbSet<Subject?> Subjects { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}