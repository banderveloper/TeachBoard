using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Subject;

public class GetAllSubjectsQuery : IRequest<IList<Domain.Entities.Subject>>
{
}

public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, IList<Domain.Entities.Subject>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSubjectsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Domain.Entities.Subject?>> Handle(GetAllSubjectsQuery request,
        CancellationToken cancellationToken) =>
        await _context.Subjects.ToListAsync(cancellationToken);
}