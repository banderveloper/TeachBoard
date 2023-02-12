using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Queries.GetPendingUserByRegisterCode;

public class
    GetPendingUserByRegisterCodeQueryHandler : IRequestHandler<GetPendingUserByRegistrationCodeQuery, PendingUser>
{
    private readonly IApplicationDbContext _context;

    public GetPendingUserByRegisterCodeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PendingUser> Handle(GetPendingUserByRegistrationCodeQuery request,
        CancellationToken cancellationToken)
    {
        // try to get pending user by register code
        var pendingUser = await _context.PendingUsers
            .FirstOrDefaultAsync(pu => pu.RegisterCode == request.RegisterCode,
                cancellationToken);

        // if not found - exception
        if (pendingUser is null)
            throw new NotFoundException
            {
                Error = "pending_user_not_found",
                ErrorDescription = $"Pending user with register code {request.RegisterCode} not found"
            };

        // if found - return it
        return pendingUser;
    }
}