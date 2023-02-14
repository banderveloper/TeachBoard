using MediatR;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.SetRefreshSession;

// Create or update refresh session. Returns new refresh token
public class SetRefreshSessionCommand : IRequest<RefreshSession>
{
    public int UserId { get; set; }
}