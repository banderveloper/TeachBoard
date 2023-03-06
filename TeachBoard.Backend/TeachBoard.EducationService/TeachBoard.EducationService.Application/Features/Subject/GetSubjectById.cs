using MediatR;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Subject;

public class GetSubjectByIdQuery : IRequest<Domain.Entities.Subject?>
{
    public int SubjectId { get; set; }
}

public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, Domain.Entities.Subject?>
{
    private readonly IApplicationDbContext _context;

    public GetSubjectByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Subject?> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken) =>
        await _context.Subjects.FindAsync(new object[] { request.SubjectId }, cancellationToken);
}