using MediatR;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Subject;

public class GetSubjectByIdQuery : IRequest<Domain.Entities.Subject>
{
    public int SubjectId { get; set; }
}


public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, Domain.Entities.Subject>
{
    private readonly IApplicationDbContext _context;

    public GetSubjectByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Subject> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects.FindAsync(request.SubjectId, cancellationToken);

        if (subject is null)
            throw new NotFoundException
            {
                Error = "subject_not_found",
                ErrorDescription = $"Subject with id '{request.SubjectId}' not found",
                ReasonField = "id"
            };

        return subject;
    }
}