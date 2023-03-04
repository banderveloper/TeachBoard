using MediatR;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Queries;

public class GetUserByIdQuery : IRequest<User>
{
    public int UserId { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
        
        return user;
    }
}