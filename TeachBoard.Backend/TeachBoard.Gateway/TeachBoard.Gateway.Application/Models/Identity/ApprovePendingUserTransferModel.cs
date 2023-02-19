namespace TeachBoard.Gateway.Application.Models.Identity;

public class ApprovePendingUserTransferModel
{
    public string RegisterCode { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
}