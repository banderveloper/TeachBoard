namespace TeachBoard.Gateway.Application.Models.Identity.Request;

public class ApprovePendingUserRequestModel
{
    public string RegisterCode { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
}