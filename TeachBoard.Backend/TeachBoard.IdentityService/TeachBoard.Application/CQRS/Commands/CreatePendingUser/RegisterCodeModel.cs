namespace TeachBoard.Application.CQRS.Commands.CreatePendingUser;

public class RegisterCodeModel
{
    public string RegisterCode { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}