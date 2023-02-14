using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.UpdateRefreshSession;

public class UpdateRefreshSessionCommandHandler : IRequestHandler<UpdateRefreshSessionCommand, RefreshSession>
{
    private readonly IApplicationDbContext _context;

    public UpdateRefreshSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshSession> Handle(UpdateRefreshSessionCommand request, CancellationToken cancellationToken)
    {
        // get session with given refresh token
        var existingSession = await _context.RefreshSessions
            .FirstOrDefaultAsync(session => session.RefreshToken == request.RefreshToken,
                cancellationToken);
        
        // if it is absent - exception
        if (existingSession is null)
            throw new NotFoundException
            {
                Error = "session_not_found",
                ErrorDescription = $"Refresh failed. Refresh token '{request.RefreshToken}' does not belong to any session"
            };
        
        // if it is - update session and return it
        existingSession.RefreshToken = Guid.NewGuid();
        existingSession.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        return existingSession;
    }
}