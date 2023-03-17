using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Domain.Entities;

namespace TeachBoard.EducationService.Application.Interfaces;

// Interface for main db context
public interface IApplicationDbContext
{
    DbSet<CompletedHomework> CompletedHomeworks { get; set; }
    DbSet<Examination> Examinations { get; set; }
    DbSet<Homework> Homeworks { get; set; }
    DbSet<Lesson> Lessons { get; set; }
    DbSet<StudentExaminationActivity> StudentExaminationActivities { get; set; }
    DbSet<StudentLessonActivity> StudentLessonActivities { get; set; }
    DbSet<Subject> Subjects { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}