using MediatR;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        // get user by id 
        var user = await _context.Users.FindAsync(request.UserId, cancellationToken);

        // if not found
        if (user is null)
            throw new NotFoundException
            {
                Error = "user_not_found",
                ErrorDescription = $"User with id {request.UserId} not found"
            };

        return user;
    }
}