using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class SetStudentLessonActivityCommand : IRequest<Domain.Entities.StudentLessonActivity>
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
    public int? Grade { get; set; }
}

public class SetStudentLessonActivityCommandHandler
    : IRequestHandler<SetStudentLessonActivityCommand, Domain.Entities.StudentLessonActivity>
{
    private readonly IApplicationDbContext _context;

    public SetStudentLessonActivityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.StudentLessonActivity> Handle(SetStudentLessonActivityCommand request,
        CancellationToken cancellationToken)
    {
        // try get existing lesson by id, and throw exception if it is not exists
        var existingLesson = await _context.Lessons.FindAsync(new object[] { request.LessonId }, cancellationToken);

        if (existingLesson is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.LessonNotFound,
                PublicErrorMessage = "Lesson not found",
                LogErrorMessage =
                    $"SetStudentLessonActivityCommand error. Lesson with id '{request.LessonId}' not found",
            };

        // if lesson is not started yet - exception
        if (existingLesson.StartsAt > DateTime.Now)
        {
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.LessonNotStarted,
                PublicErrorMessage = "Lesson is not started yet, impossible to set student activity",
                LogErrorMessage =
                    $"SetStudentLessonActivityCommand error. Lesson is not started. Expected start time: [{existingLesson.StartsAt.ToUniversalTime()}], request time: [{DateTime.Now.ToUniversalTime()}]"
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
            existingActivity = new Domain.Entities.StudentLessonActivity
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