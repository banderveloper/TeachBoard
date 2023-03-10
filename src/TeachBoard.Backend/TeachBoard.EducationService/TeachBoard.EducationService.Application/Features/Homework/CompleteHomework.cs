using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Entities;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class CompleteHomeworkCommand : IRequest<CompletedHomework>
{
    public int HomeworkId { get; set; }
    public int StudentId { get; set; }
    public int StudentGroupId { get; set; }
    public string? FilePath { get; set; }
    public string? StudentComment { get; set; }
}

public class CompleteHomeworkCommandHandler : IRequestHandler<CompleteHomeworkCommand, CompletedHomework>
{
    private readonly IApplicationDbContext _context;

    public CompleteHomeworkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CompletedHomework> Handle(CompleteHomeworkCommand request, CancellationToken cancellationToken)
    {
        // if homework with given id does not exists - exception
        var existingHomework =
            await _context.Homeworks.FindAsync(new object[] { request.HomeworkId }, cancellationToken);

        if (existingHomework is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.HomeworkNotFound,
                PublicErrorMessage = "Homework not found",
                LogErrorMessage = $"CompleteHomeworkCommand error. Homework with id [{request.HomeworkId}] not found",
            };

        // if student's group not equals to homework group id 
        if (existingHomework.GroupId != request.StudentGroupId)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.HomeworkNotFound,
                PublicErrorMessage = "Homework to given group not found",
                LogErrorMessage =
                    $"CompleteHomeworkCommand error. Homework with id '{request.HomeworkId}' to group with id '{request.StudentGroupId}' not found",
            };

        // if student with given id already completed homework with given id - exception

        var existingCompletedHomework = await _context.CompletedHomeworks
            .FirstOrDefaultAsync(ch => ch.HomeworkId == request.HomeworkId &&
                                       ch.StudentId == request.StudentId, cancellationToken);

        if (existingCompletedHomework is not null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.CompletedHomeworkAlreadyExists,
                PublicErrorMessage = "Given homework is already completed by student",
                LogErrorMessage =
                    $"CompleteHomeworkCommand error. Student with id '{request.StudentId}' has already completed homework with id '{request.HomeworkId}'"
            };

        // if all ok - complete homework, create entity
        var completedHomework = new CompletedHomework
        {
            HomeworkId = request.HomeworkId,
            StudentId = request.StudentId,
            StudentComment = request.StudentComment,
            FilePath = request.FilePath,
            CheckingTeacherId = existingHomework.TeacherId
        };
        _context.CompletedHomeworks.Add(completedHomework);
        await _context.SaveChangesAsync(cancellationToken);

        return completedHomework;
    }
}