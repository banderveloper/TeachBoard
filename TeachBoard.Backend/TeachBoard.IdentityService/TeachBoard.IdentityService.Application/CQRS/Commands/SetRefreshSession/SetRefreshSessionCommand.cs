using MediatR;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.SetRefreshSession;

// Create or update refresh session. Returns new refresh token
public class SetRefreshSessionCommand : IRequest<Guid>
{
    public int UserId { get; set; }
}