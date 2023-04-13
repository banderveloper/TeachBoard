using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class UpdateLessonTopicCommand : IRequest<Domain.Entities.Lesson>
{
    public int LessonId { get; set; }
    public string Topic { get; set; }
}

public class UpdateLessonTopicCommandHandler : IRequestHandler<UpdateLessonTopicCommand, Domain.Entities.Lesson>
{
    private readonly IApplicationDbContext _context;

    public UpdateLessonTopicCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Lesson> Handle(UpdateLessonTopicCommand request, CancellationToken cancellationToken)
    {
        var lesson = await _context.Lessons.AsTracking()
            .FirstOrDefaultAsync(l => l.Id == request.LessonId, cancellationToken);

        if (lesson is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.LessonNotFound,
                PublicErrorMessage = "Lesson not found",
                ReasonField = "id"
            };

        lesson.Topic = request.Topic;

        await _context.SaveChangesAsync(cancellationToken);

        return lesson;
    }
}