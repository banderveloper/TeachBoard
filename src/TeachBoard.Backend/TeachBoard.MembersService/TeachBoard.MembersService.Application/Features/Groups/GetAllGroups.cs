using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Groups;

// Query
public class GetAllGroupsQuery : IRequest<IList<Group>>
{
}

// Handler
public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, IList<Group>>
{
    private readonly IApplicationDbContext _context;

    public GetAllGroupsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Group>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _context.Groups.ToListAsync(cancellationToken);

        return groups;
    }
}