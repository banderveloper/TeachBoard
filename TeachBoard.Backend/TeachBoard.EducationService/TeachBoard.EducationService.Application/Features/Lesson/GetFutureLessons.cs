using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetFutureLessonsQuery : IRequest<LessonsListModel>
{
}

public class GetFutureLessonsQueryHandler : IRequestHandler<GetFutureLessonsQuery, LessonsListModel>
{
    private readonly  IApplicationDbContext _context;

    public GetFutureLessonsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LessonsListModel> Handle(GetFutureLessonsQuery request, CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .Where(l => l.StartsAt > DateTime.Now)
            .ToListAsync(cancellationToken);

        return new LessonsListModel { Lessons = lessons };
    }
}