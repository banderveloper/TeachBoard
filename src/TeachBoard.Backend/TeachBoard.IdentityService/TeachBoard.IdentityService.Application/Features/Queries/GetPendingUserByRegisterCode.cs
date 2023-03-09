using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.Features.Queries;

public class GetPendingUserByRegistrationCodeQuery : IRequest<PendingUser?>
{
    public string RegisterCode { get; set; } = string.Empty;
}

public class GetPendingUserByRegisterCodeQueryHandler : IRequestHandler<GetPendingUserByRegistrationCodeQuery, PendingUser?>
{
    private readonly IApplicationDbContext _context;

    public GetPendingUserByRegisterCodeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PendingUser?> Handle(GetPendingUserByRegistrationCodeQuery request,
        CancellationToken cancellationToken)
    {
        // try to get pending user by register code
        var pendingUser = await _context.PendingUsers
            .FirstOrDefaultAsync(pu => pu.RegisterCode == request.RegisterCode,
                cancellationToken);
        
        // if found - return it
        return pendingUser;
    }
}