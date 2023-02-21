using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.StudentLessonActivity;

public class GetStudentLessonActivitiesByStudentIdQuery : IRequest<StudentLessonActivitiesListModel>
{
    public int StudentId { get; set; }
}

public class GetStudentLessonActivitiesByStudentIdQueryHandler
    : IRequestHandler<GetStudentLessonActivitiesByStudentIdQuery, StudentLessonActivitiesListModel>
{
    private readonly IApplicationDbContext _context;

    public GetStudentLessonActivitiesByStudentIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentLessonActivitiesListModel> Handle(GetStudentLessonActivitiesByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        var activities = await _context.StudentLessonActivities
            .Where(sla => sla.StudentId == request.StudentId)
            .ToListAsync(cancellationToken);

        if (activities.Count == 0)
            throw new NotFoundException
            {
                Error = "student_lesson_activities_not_found",
                ErrorDescription = $"Student lesson activities with student id '{request.StudentId}' not found",
                ReasonField = "studentId"
            };

        return new StudentLessonActivitiesListModel { StudentLessonActivities = activities };
    }
}