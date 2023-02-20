using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Subject;

public class GetAllSubjectsQuery : IRequest<SubjectsListModel>
{
}

public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, SubjectsListModel>
{
    private readonly IApplicationDbContext _context;

    public GetAllSubjectsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SubjectsListModel> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
    {
        var subjects = await _context.Subjects.ToListAsync(cancellationToken);

        if (subjects.Count == 0)
            throw new NotFoundException
            {
                Error = "subjects_not_found",
                ErrorDescription = "Subjects not found"
            };

        return new SubjectsListModel { Subjects = subjects };
    }
}

public class SubjectsListModel
{
    public IList<Domain.Entities.Subject> Subjects { get; set; }
}