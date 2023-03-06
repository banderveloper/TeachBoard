using MediatR;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Subject;

public class DeleteSubjectByIdCommand : IRequest<bool>
{
    public int SubjectId { get; set; }
}

public class DeleteSubjectByIdCommandHandler : IRequestHandler<DeleteSubjectByIdCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteSubjectByIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteSubjectByIdCommand request, CancellationToken cancellationToken)
    {
        var subjectToDelete = await _context.Subjects.FindAsync(new object[] { request.SubjectId }, cancellationToken);

        if (subjectToDelete is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.SubjectNotFound,
                PublicErrorMessage = "Subject not found",
                LogErrorMessage = $"DeleteSubjectByIdCommand error. Subject with id '{request.SubjectId}' not found",
            };

        _context.Subjects.Remove(subjectToDelete);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}