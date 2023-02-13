using MediatR;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Queries.GetUserByCredentials;

public class GetUserByCredentialsQuery : IRequest<User>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}