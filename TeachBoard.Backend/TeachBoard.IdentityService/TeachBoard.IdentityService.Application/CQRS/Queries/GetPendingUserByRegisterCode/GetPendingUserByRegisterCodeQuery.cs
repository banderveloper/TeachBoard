using MediatR;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Queries.GetPendingUserByRegisterCode;

public class GetPendingUserByRegistrationCodeQuery : IRequest<PendingUser>
{
    public string RegisterCode { get; set; } = string.Empty;
}