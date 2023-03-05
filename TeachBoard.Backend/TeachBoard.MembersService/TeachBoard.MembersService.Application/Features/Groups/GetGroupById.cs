using MediatR;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Groups;

// Query
public class GetGroupByIdQuery : IRequest<Group?>
{
    public int GroupId { get; set; }
}

// Handler
public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, Group?>
{
    private readonly IApplicationDbContext _context;

    public GetGroupByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Group?> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await _context.Groups.FindAsync(new object[] { request.GroupId }, cancellationToken);
        
        return group;
    }
}