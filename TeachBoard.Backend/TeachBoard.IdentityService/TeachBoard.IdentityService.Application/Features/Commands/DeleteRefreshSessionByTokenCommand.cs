using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;

namespace TeachBoard.IdentityService.Application.Features.Commands;

public class DeleteRefreshSessionByTokenCommand : IRequest<bool>
{
    public Guid RefreshToken { get; set; }
}

public class DeleteRefreshSessionByTokenCommandHandler : IRequestHandler<DeleteRefreshSessionByTokenCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteRefreshSessionByTokenCommandHandler(IApplicationDbContext context) =>
        _context = context;
    
    public async Task<bool> Handle(DeleteRefreshSessionByTokenCommand request, CancellationToken cancellationToken)
    {
        // get existing session by refresh token
        var existingSession = await _context.RefreshSessions
            .FirstOrDefaultAsync(session => session.RefreshToken == request.RefreshToken,
                cancellationToken);

        // if not exists - error
        if (existingSession is null)
            throw new ExpectedApiException
            {
                ErrorCode = "session_not_found",
                PublicErrorMessage = "Session bound to given token not found",
                LogErrorMessage =
                    $"Delete refresh session by refresh error. Refresh token ['{request.RefreshToken}'] not found"
            };

        // if found - delete session from db
        _context.RefreshSessions.Remove(existingSession);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}