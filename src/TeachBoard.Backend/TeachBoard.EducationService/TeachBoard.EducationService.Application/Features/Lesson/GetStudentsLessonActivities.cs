using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetStudentsLessonActivitiesQuery : IRequest<IList<Domain.Entities.StudentLessonActivity>>
{
    public int LessonId { get; set; }
    public IList<int> StudentIds { get; set; }
}

public class GetStudentsLessonActivitiesQueryHandler : IRequestHandler<GetStudentsLessonActivitiesQuery,
    IList<Domain.Entities.StudentLessonActivity>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentsLessonActivitiesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Domain.Entities.StudentLessonActivity>> Handle(GetStudentsLessonActivitiesQuery request,
        CancellationToken cancellationToken) =>
        await _context.StudentLessonActivities.Where(activity =>
                activity.LessonId == request.LessonId && request.StudentIds.Contains(activity.StudentId))
            .ToListAsync(cancellationToken);
}