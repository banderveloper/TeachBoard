using MediatR;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.DeleteRefreshSessionByToken;

public class DeleteRefreshSessionByTokenCommand : IRequest
{
    public Guid RefreshToken { get; set; }
}