using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetCurrentLessonPresentationByTeacherIdQuery : IRequest<LessonPresentationDataModel?>
{
    public int TeacherId { get; set; }
}

public class
    GetCurrentLessonPresentationByTeacherIdQueryHandler : IRequestHandler<GetCurrentLessonPresentationByTeacherIdQuery,
        LessonPresentationDataModel?>
{
    private readonly IApplicationDbContext _context;

    public GetCurrentLessonPresentationByTeacherIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LessonPresentationDataModel?> Handle(GetCurrentLessonPresentationByTeacherIdQuery request,
        CancellationToken cancellationToken)
        => await _context.Lessons
            .Include(l => l.Subject)
            .Select(l => new LessonPresentationDataModel
            {
                Id = l.Id,
                TeacherId = l.TeacherId,
                SubjectName = l.Subject.Name,
                Classroom = l.Classroom,
                Topic = l.Topic,
                StartsAt = l.StartsAt,
                EndsAt = l.EndsAt,
                GroupId = l.GroupId
            })
            .FirstOrDefaultAsync(l => l.TeacherId == request.TeacherId &&
                                      DateTime.Now >= l.StartsAt &&
                                      DateTime.Now <= l.EndsAt,
                cancellationToken);
}