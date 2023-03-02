using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.Application.Features.StudentLessonActivity;

public class GetStudentLessonActivitiesByStudentIdQuery : IRequest<StudentLessonActivityPublicListModel>
{
    public int StudentId { get; set; }
}

public class GetStudentLessonActivitiesByStudentIdQueryHandler
    : IRequestHandler<GetStudentLessonActivitiesByStudentIdQuery, StudentLessonActivityPublicListModel>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLessonActivitiesByStudentIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentLessonActivityPublicListModel> Handle(GetStudentLessonActivitiesByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        var activities = await _context.StudentLessonActivities
            .Include(sla => sla.Lesson)
            .ThenInclude(lesson => lesson.Subject)
            .Where(sla => sla.StudentId == request.StudentId)
            .Select(sla => new StudentLessonActivityPublicModel()
            {
                AttendanceStatus = sla.AttendanceStatus,
                LessonId = sla.LessonId,
                LessonTopic = sla.Lesson.Topic,
                SubjectName = sla.Lesson.Subject.Name,
                Grade = sla.Grade,
                ActivityCreatedAt = sla.CreatedAt
            })
            .ToListAsync(cancellationToken);

        if (activities.Count == 0)
            throw new NotFoundException
            {
                Error = "student_lesson_activities_not_found",
                ErrorDescription = $"Student lesson activities with student id '{request.StudentId}' not found",
                ReasonField = "studentId"
            };

        return new StudentLessonActivityPublicListModel { Activities = activities };
    }
}

public class StudentLessonActivityPublicModel
{
    public int LessonId { get; set; }
    public string LessonTopic { get; set; }
    public string SubjectName { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
    public int? Grade { get; set; }
    public DateTime ActivityCreatedAt { get; set; }
}

public class StudentLessonActivityPublicListModel
{
    public IList<StudentLessonActivityPublicModel> Activities { get; set; }
}