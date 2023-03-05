using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Groups;

// Query
public class GetAllGroupsQuery : IRequest<GroupsListModel>
{
}

// Handler
public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, GroupsListModel>
{
    private IApplicationDbContext _context;

    public GetAllGroupsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GroupsListModel> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _context.Groups.ToListAsync(cancellationToken);

        return new GroupsListModel
        {
            Groups = groups
        };
    }
}

// Return model
public class GroupsListModel
{
    public IList<Group> Groups { get; set; }
}