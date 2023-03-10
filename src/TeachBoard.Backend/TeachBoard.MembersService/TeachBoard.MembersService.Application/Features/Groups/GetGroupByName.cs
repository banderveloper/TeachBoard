using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Groups;

// Query
public class GetGroupByNameQuery : IRequest<Group?>
{
    public string GroupName { get; set; } = string.Empty;
}

// Handler
public class GetGroupByNameQueryHandler : IRequestHandler<GetGroupByNameQuery, Group?>
{
    private readonly IApplicationDbContext _context;

    public GetGroupByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Group?> Handle(GetGroupByNameQuery request, CancellationToken cancellationToken)
    {
        var group = await _context.Groups
            .FirstOrDefaultAsync(g => string.Equals(g.Name.ToLower(), request.GroupName.ToLower()),
                cancellationToken);

        return group;
    }
}