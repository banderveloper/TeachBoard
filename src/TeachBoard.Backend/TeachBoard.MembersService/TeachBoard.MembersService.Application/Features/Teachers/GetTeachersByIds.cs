using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Teachers;

public class GetTeachersByIdsQuery : IRequest<IList<Teacher>>
{
    public IList<int> Ids { get; set; }
}

public class GetTeachersByIdsQueryHandler : IRequestHandler<GetTeachersByIdsQuery, IList<Teacher>>
{
    private readonly IApplicationDbContext _context;

    public GetTeachersByIdsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Teacher>> Handle(GetTeachersByIdsQuery request, CancellationToken cancellationToken)
    {
        var teachers = await _context.Teachers
            .Where(t => request.Ids.Contains(t.Id))
            .ToListAsync(cancellationToken);

        return teachers;
    }
}