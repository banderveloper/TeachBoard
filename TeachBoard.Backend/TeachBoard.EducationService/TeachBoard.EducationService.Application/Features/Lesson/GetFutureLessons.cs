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
    private IApplicationDbContext _context;

    public GetFutureLessonsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LessonsListModel> Handle(GetFutureLessonsQuery request, CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .Where(l => l.StartsAt > DateTime.Now)
            .ToListAsync(cancellationToken);

        if (lessons.Count == 0)
            throw new NotFoundException
            {
                Error = "lessons_not_found",
                ErrorDescription = $"Future lessons (from {DateTime.Now.ToUniversalTime()}) not found"
            };

        return new LessonsListModel { Lessons = lessons };
    }
}