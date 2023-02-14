using MediatR;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.UpdateRefreshSession;

public class UpdateRefreshSessionCommand : IRequest<RefreshSession>
{
    public Guid RefreshToken { get; set; }
}