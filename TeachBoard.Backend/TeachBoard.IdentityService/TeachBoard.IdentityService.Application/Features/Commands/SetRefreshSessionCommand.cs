using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.Features.Commands;

// Create or update refresh session. Returns new refresh token

public class SetRefreshSessionCommand : IRequest<RefreshSession>
{
    public int UserId { get; set; }
}

public class SetRefreshSessionCommandHandler : IRequestHandler<SetRefreshSessionCommand, RefreshSession>
{
    private readonly IApplicationDbContext _context;

    public SetRefreshSessionCommandHandler(IApplicationDbContext context) =>
        _context = context;
    
    public async Task<RefreshSession> Handle(SetRefreshSessionCommand request, CancellationToken cancellationToken)
    {
        // try to get existing user session by user id
        var existingSession = await _context.RefreshSessions
            .AsTracking()
            .FirstOrDefaultAsync(
                session => session.UserId == request.UserId,
                cancellationToken);

        // generate new refresh token
        var newRefreshToken = Guid.NewGuid();

        // if session exists - update token
        if (existingSession is not null)
        {
            existingSession.RefreshToken = newRefreshToken;
            existingSession.UpdatedAt = DateTime.Now;
        }
        // if not exists - create
        else
        {
            existingSession = new RefreshSession
            {
                UserId = request.UserId,
                RefreshToken = newRefreshToken
            };
            _context.RefreshSessions.Add(existingSession);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return existingSession;
    }
}