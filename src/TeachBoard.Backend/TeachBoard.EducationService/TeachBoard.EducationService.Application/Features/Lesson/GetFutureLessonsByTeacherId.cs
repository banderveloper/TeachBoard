using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetFutureLessonsByTeacherIdQuery : IRequest<IList<Domain.Entities.Lesson>>
{
    public int TeacherId { get; set; }
}

public class
    GetFutureLessonsByTeacherIdQueryHandler : IRequestHandler<GetFutureLessonsByTeacherIdQuery,
        IList<Domain.Entities.Lesson>>
{
    private readonly IApplicationDbContext _context;

    public GetFutureLessonsByTeacherIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Domain.Entities.Lesson>> Handle(GetFutureLessonsByTeacherIdQuery request,
        CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .Where(l => l.StartsAt > DateTime.Now && l.TeacherId == request.TeacherId)
            .ToListAsync(cancellationToken);

        return lessons;
    }
}