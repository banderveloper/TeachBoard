using MediatR;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<User>
{
    public int UserId { get; set; }
}