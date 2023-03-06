using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetLessonsByGroupIdQuery : IRequest<LessonsListModel>
{
    public int GroupId { get; set; }
}

public class GetLessonsByGroupIdQueryHandler : IRequestHandler<GetLessonsByGroupIdQuery, LessonsListModel>
{
    private readonly IApplicationDbContext _context;

    public GetLessonsByGroupIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LessonsListModel> Handle(GetLessonsByGroupIdQuery request, CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .Where(l => l.GroupId == request.GroupId)
            .ToListAsync(cancellationToken);
        
        return new LessonsListModel { Lessons = lessons };
    }
}