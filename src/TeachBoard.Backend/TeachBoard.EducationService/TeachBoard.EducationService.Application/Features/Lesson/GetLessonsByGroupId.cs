using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class GetLessonsPresentationDataByGroupIdQuery : IRequest<IList<LessonPresentationDataModel>>
{
    public int GroupId { get; set; }
}

public class GetLessonsPresentationDataByGroupIdQueryHandler : IRequestHandler<GetLessonsPresentationDataByGroupIdQuery,
    IList<LessonPresentationDataModel>>
{
    private readonly IApplicationDbContext _context;

    public GetLessonsPresentationDataByGroupIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<LessonPresentationDataModel>> Handle(GetLessonsPresentationDataByGroupIdQuery request,
        CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .Include(l => l.Subject)
            .Where(l => l.GroupId == request.GroupId)
            .Select(l => new LessonPresentationDataModel
            {
                SubjectName = l.Subject.Name,
                TeacherId = l.TeacherId,
                Classroom = l.Classroom,
                Topic = l.Topic,
                StartsAt = l.StartsAt,
                EndsAt = l.EndsAt,
                Id = l.Id,
                GroupId = l.GroupId
            })
            .ToListAsync(cancellationToken);

        return lessons;
    }
}