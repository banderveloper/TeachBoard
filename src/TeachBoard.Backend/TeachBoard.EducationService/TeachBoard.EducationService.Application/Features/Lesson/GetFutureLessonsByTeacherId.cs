using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetFutureLessonsByTeacherIdQuery : IRequest<IList<FutureLessonPresentationModel>>
{
    public int TeacherId { get; set; }
}

public class GetFutureLessonsByTeacherIdQueryHandler : IRequestHandler<GetFutureLessonsByTeacherIdQuery,
        IList<FutureLessonPresentationModel>>
{
    private readonly IApplicationDbContext _context;

    public GetFutureLessonsByTeacherIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<FutureLessonPresentationModel>> Handle(GetFutureLessonsByTeacherIdQuery request,
        CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .Include(l => l.Subject)
            .Where(l => l.StartsAt > DateTime.Now && l.TeacherId == request.TeacherId)
            .Select(l => new FutureLessonPresentationModel()
            {
                Classroom = l.Classroom,
                StartsAt = l.StartsAt,
                EndsAt = l.EndsAt,
                GroupId = l.GroupId,
                SubjectName = l.Subject.Name,
                TeacherId = l.TeacherId
            })
            .ToListAsync(cancellationToken);

        return lessons;
    }
}

public class FutureLessonPresentationModel
{
    public string SubjectName { get; set; }
    public int TeacherId { get; set; }
    public int GroupId { get; set; }
    public string Classroom { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}