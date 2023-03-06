using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class GetHomeworksByGroupIdQuery : IRequest<IList<Domain.Entities.Homework>>
{
    public int GroupId { get; set; }
}

public class GetHomeworksByGroupIdQueryHandler : IRequestHandler<GetHomeworksByGroupIdQuery, IList<Domain.Entities.Homework>>
{
    private readonly IApplicationDbContext _context;

    public GetHomeworksByGroupIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Domain.Entities.Homework>> Handle(GetHomeworksByGroupIdQuery request,
        CancellationToken cancellationToken)
    {
        var groupHomeworks = await _context.Homeworks
            .Where(h => h.GroupId == request.GroupId)
            .ToListAsync(cancellationToken);

        return groupHomeworks;
    }
}