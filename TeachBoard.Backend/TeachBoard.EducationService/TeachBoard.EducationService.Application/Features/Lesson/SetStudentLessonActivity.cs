using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Entities;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class SetStudentLessonActivityCommand : IRequest<StudentLessonActivity>
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
    public int? Grade { get; set; }
}

public class SetStudentLessonActivityCommandHandler
    : IRequestHandler<SetStudentLessonActivityCommand, StudentLessonActivity>
{
    private readonly IApplicationDbContext _context;

    public SetStudentLessonActivityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentLessonActivity> Handle(SetStudentLessonActivityCommand request,
        CancellationToken cancellationToken)
    {
        // try get existing lesson by id, and throw exception if it is not exists
        var existingLesson = await _context.Lessons.FindAsync(request.LessonId, cancellationToken);
        if (existingLesson is null)
            throw new NotFoundException
            {
                Error = "lesson_not_found",
                ErrorDescription = $"Lesson with id '{request.LessonId}' not found",
                ReasonField = "id"
            };

        // if lesson is not started yet - exception
        if (existingLesson.StartsAt > DateTime.Now)
        {
            throw new InvalidDateTimeException
            {
                Error = "lesson_not_started",
                ErrorDescription = "Lesson is not started, impossible to set student activity"
            };
        }
        
        // try get student activity
        var existingActivity = await _context.StudentLessonActivities
            .FirstOrDefaultAsync(sla => sla.LessonId == request.LessonId &&
                                        sla.StudentId == request.StudentId,
                cancellationToken);

        // if exists - update it
        if (existingActivity is not null)
        {
            existingActivity.AttendanceStatus = request.AttendanceStatus;
            existingActivity.Grade = request.Grade;
        }
        // if not exists - create
        else
        {
            existingActivity = new StudentLessonActivity
            {
                LessonId = request.LessonId,
                StudentId = request.StudentId,
                AttendanceStatus = request.AttendanceStatus,
                Grade = request.Grade
            };

            _context.StudentLessonActivities.Add(existingActivity);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return existingActivity;
    }
}