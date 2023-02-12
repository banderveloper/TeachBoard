using MediatR;
using TeachBoard.IdentityService.Domain.Enums;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.CreatePendingUser;

// Command for CreatePendingUserCommandHandler
public class CreatePendingUserCommand : IRequest<RegisterCodeModel>
{
    public Role Role { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }

    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
}