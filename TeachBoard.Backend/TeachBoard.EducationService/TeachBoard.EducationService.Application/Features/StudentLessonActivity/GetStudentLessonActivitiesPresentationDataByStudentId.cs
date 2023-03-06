using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.Application.Features.StudentLessonActivity;

public class GetStudentLessonActivitiesPresentationDataByStudentIdQuery : IRequest<IList<StudentLessonActivityPresentationDataModel>>
{
    public int StudentId { get; set; }
}

public class GetStudentLessonActivitiesPresentationDataByStudentIdQueryHandler
    : IRequestHandler<GetStudentLessonActivitiesPresentationDataByStudentIdQuery, IList<StudentLessonActivityPresentationDataModel>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLessonActivitiesPresentationDataByStudentIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<StudentLessonActivityPresentationDataModel>> Handle(GetStudentLessonActivitiesPresentationDataByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        var activities = await _context.StudentLessonActivities
            .Include(sla => sla.Lesson)
            .ThenInclude(lesson => lesson.Subject)
            .Where(sla => sla.StudentId == request.StudentId)
            .Select(sla => new StudentLessonActivityPresentationDataModel
            {
                StudentId = sla.StudentId,
                AttendanceStatus = sla.AttendanceStatus,
                LessonId = sla.LessonId,
                LessonTopic = sla.Lesson.Topic,
                SubjectName = sla.Lesson.Subject.Name,
                Grade = sla.Grade,
                ActivityCreatedAt = sla.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return activities;
    }
}

public class StudentLessonActivityPresentationDataModel
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public string LessonTopic { get; set; }
    public string SubjectName { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
    public int? Grade { get; set; }
    public DateTime ActivityCreatedAt { get; set; }
}