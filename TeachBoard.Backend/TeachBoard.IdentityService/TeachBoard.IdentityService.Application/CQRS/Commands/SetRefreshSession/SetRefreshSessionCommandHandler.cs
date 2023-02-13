using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.SetRefreshSession;

// Create or update refresh session. Returns new refresh token
public class SetRefreshSessionCommandHandler : IRequestHandler<SetRefreshSessionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public SetRefreshSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(SetRefreshSessionCommand request, CancellationToken cancellationToken)
    {
        // try to get existing user session by user id
        var existingSession = await _context.RefreshSessions
            .FirstOrDefaultAsync(session => session.UserId == request.UserId,
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
            _context.RefreshSessions.Add(new RefreshSession
            {
                UserId = request.UserId,
                RefreshToken = newRefreshToken
            });
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        return newRefreshToken;
    }
}