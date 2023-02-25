using MediatR;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Entities;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class CheckHomeworkCommand : IRequest<CompletedHomework>
{
    public int TeacherId { get; set; }
    public int CompletedHomeworkId { get; set; }
    public int Grade { get; set; }
    public string? Comment { get; set; }
}

public class CheckHomeworkCommandHandler : IRequestHandler<CheckHomeworkCommand, CompletedHomework>
{
    private readonly IApplicationDbContext _context;

    public CheckHomeworkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompletedHomework> Handle(CheckHomeworkCommand request, CancellationToken cancellationToken)
    {
        var completedHomework = await _context.CompletedHomeworks
            .FindAsync(request.CompletedHomeworkId, cancellationToken);

        if (completedHomework is null)
            throw new NotFoundException
            {
                Error = "completed_homework_not_found",
                ErrorDescription = $"Completed homework with id '{request.CompletedHomeworkId}' not found",
                ReasonField = "id"
            };

        // If homework was added by another teacher - exception
        if (completedHomework.CheckingTeacherId != request.TeacherId)
            throw new HomeworkAccessException
            {
                Error = "completed_homework_invalid_teacher",
                ErrorDescription =
                    $"Completed homework with id '{completedHomework.Id}' was added by another teacher, checking is denied",
                ReasonField = "teacherId"
            };

        completedHomework.Grade = request.Grade;
        completedHomework.CheckingTeacherComment = request.Comment;

        await _context.SaveChangesAsync(cancellationToken);

        return completedHomework;
    }
}