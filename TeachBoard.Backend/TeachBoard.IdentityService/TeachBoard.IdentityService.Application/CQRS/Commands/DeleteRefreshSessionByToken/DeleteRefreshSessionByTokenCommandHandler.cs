using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.DeleteRefreshSessionByToken;

public class DeleteRefreshSessionByTokenCommandHandler : IRequestHandler<DeleteRefreshSessionByTokenCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteRefreshSessionByTokenCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRefreshSessionByTokenCommand request, CancellationToken cancellationToken)
    {
        // get existing session by refresh token
        var existingSession = await _context.RefreshSessions
            .FirstOrDefaultAsync(session => session.RefreshToken == request.RefreshToken,
                cancellationToken);

        // if not exists - error
        if (existingSession is null)
            throw new NotFoundException
            {
                Error = "session_not_found",
                ErrorDescription = $"Refresh token '{request.RefreshToken}' not found at any session"
            };

        // if found - delete session from db
        _context.RefreshSessions.Remove(existingSession);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}