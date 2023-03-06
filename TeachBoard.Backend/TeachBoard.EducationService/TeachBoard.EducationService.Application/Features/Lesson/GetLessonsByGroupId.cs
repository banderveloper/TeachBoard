using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetLessonsByGroupIdQuery : IRequest<IList<Domain.Entities.Lesson>>
{
    public int GroupId { get; set; }
}

public class GetLessonsByGroupIdQueryHandler : IRequestHandler<GetLessonsByGroupIdQuery, IList<Domain.Entities.Lesson>>
{
    private readonly IApplicationDbContext _context;

    public GetLessonsByGroupIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Domain.Entities.Lesson>> Handle(GetLessonsByGroupIdQuery request, CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .Where(l => l.GroupId == request.GroupId)
            .ToListAsync(cancellationToken);

        return lessons;
    }
}