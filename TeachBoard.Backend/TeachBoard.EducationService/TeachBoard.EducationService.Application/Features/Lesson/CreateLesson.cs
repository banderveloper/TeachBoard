using MediatR;
using TeachBoard.EducationService.Application.Configurations;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Lesson;

public class CreateLessonCommand : IRequest<Domain.Entities.Lesson>
{
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public int GroupId { get; set; }
    public string? Topic { get; set; }
    public string? Classroom { get; set; }

    public DateTime StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
}


public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, Domain.Entities.Lesson>
{
    private readonly IApplicationDbContext _context;
    private readonly LessonConfiguration _lessonConfiguration;

    public CreateLessonCommandHandler(IApplicationDbContext context, LessonConfiguration lessonConfiguration)
    {
        _context = context;
        _lessonConfiguration = lessonConfiguration;
    }

    public async Task<Domain.Entities.Lesson> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        var existingSubject = await _context.Subjects.FindAsync(request.SubjectId, cancellationToken);

        if (existingSubject is null)
            throw new NotFoundException
            {
                Error = "subject_not_found",
                ErrorDescription = $"Subject with id '{request.SubjectId}' not found",
                ReasonField = "id"
            };

        var newLesson = new Domain.Entities.Lesson
        {
            Topic = request.Topic,
            SubjectId = request.SubjectId,
            Classroom = request.Classroom,
            GroupId = request.GroupId,
            TeacherId = request.TeacherId,
            StartsAt = request.StartsAt,
            // if end time not set - set StartTime + 80 minutes (for example)
            EndsAt = request.EndsAt ?? request.StartsAt.AddMinutes(_lessonConfiguration.DefaultDurabilityMinutes),
        };

        _context.Lessons.Add(newLesson);
        await _context.SaveChangesAsync(cancellationToken);

        return newLesson;
    }
}