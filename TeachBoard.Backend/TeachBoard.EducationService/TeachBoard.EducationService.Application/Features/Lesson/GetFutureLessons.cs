using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetFutureLessonsQuery : IRequest<IList<Domain.Entities.Lesson>>
{
}

public class GetFutureLessonsQueryHandler : IRequestHandler<GetFutureLessonsQuery, IList<Domain.Entities.Lesson>>
{
    private readonly  IApplicationDbContext _context;

    public GetFutureLessonsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Domain.Entities.Lesson>> Handle(GetFutureLessonsQuery request, CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .Where(l => l.StartsAt > DateTime.Now)
            .ToListAsync(cancellationToken);

        return lessons;
    }
}