using MediatR;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.ApprovePendingUser;

// Command for ApprovePendingUserCommandHandler
public class ApprovePendingUserCommand : IRequest
{
    public string RegisterCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}