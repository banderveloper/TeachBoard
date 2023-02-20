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
        var subjectToDelete = await _context.Subjects.FindAsync(new object?[] { request.SubjectId, cancellationToken }, 
            cancellationToken);

        if (subjectToDelete is null)
            throw new NotFoundException
            {
                Error = "subject_not_found",
                ErrorDescription = $"Subject with id '{request.SubjectId}' not found",
                ReasonField = "id"
            };

        _context.Subjects.Remove(subjectToDelete);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}