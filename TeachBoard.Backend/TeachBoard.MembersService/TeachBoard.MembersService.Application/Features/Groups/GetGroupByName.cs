using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Groups;

// Query
public class GetGroupByNameQuery : IRequest<Group>
{
    public string GroupName { get; set; } = string.Empty;
}

// Handler
public class GetGroupByNameQueryHandler : IRequestHandler<GetGroupByNameQuery, Group>
{
    private readonly IApplicationDbContext _context;

    public GetGroupByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Group> Handle(GetGroupByNameQuery request, CancellationToken cancellationToken)
    {
        request.GroupName = request.GroupName.Trim().Normalize();

        var group = await _context.Groups
            .FirstOrDefaultAsync(g => g.Name.ToLower() == request.GroupName.ToLower(),
                cancellationToken);

        if (group is null)
            throw new NotFoundException
            {
                Error = "group_not_found",
                ErrorDescription = $"Group with name {request.GroupName} not found",
                ReasonField = "name"
            };

        return group;
    }
}