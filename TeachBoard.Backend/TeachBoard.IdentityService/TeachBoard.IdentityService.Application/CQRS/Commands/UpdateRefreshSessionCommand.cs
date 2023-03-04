using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Commands;

// Update session (recreate refresh) using given refresh token

public class UpdateRefreshSessionCommand : IRequest<RefreshSession>
{
    public Guid RefreshToken { get; set; }
}

public class UpdateRefreshSessionCommandHandler : IRequestHandler<UpdateRefreshSessionCommand, RefreshSession>
{
    private readonly IApplicationDbContext _context;

    public UpdateRefreshSessionCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<RefreshSession> Handle(UpdateRefreshSessionCommand request, CancellationToken cancellationToken)
    {
        // get session with given refresh token
        var existingSession = await _context.RefreshSessions
            .FirstOrDefaultAsync(
                session => session.RefreshToken == request.RefreshToken,
                cancellationToken);

        // if it is absent - exception
        if (existingSession is null)
            throw new ExpectedApiException
            {
                ErrorCode = "session_not_found",
                PublicErrorMessage = "Session bound to given token not found",
                LogErrorMessage = $"Update refresh session error. Refresh token ['{request.RefreshToken}'] not found"
            };

        // if session exists - update it and return
        existingSession.RefreshToken = Guid.NewGuid();
        existingSession.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        return existingSession;
    }
}