using MediatR;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetLessonByIdQuery : IRequest<Domain.Entities.Lesson?>
{
    public int LessonId { get; set; }
}

public class GetLessonByIdQueryHandler : IRequestHandler<GetLessonByIdQuery, Domain.Entities.Lesson?>
{
    private readonly IApplicationDbContext _context;

    public GetLessonByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Lesson?> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        => await _context.Lessons.FindAsync(new object[] { request.LessonId }, cancellationToken);
}