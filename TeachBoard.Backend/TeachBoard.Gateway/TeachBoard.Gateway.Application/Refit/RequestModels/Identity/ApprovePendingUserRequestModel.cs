namespace TeachBoard.Gateway.Application.Refit.RequestModels.Identity;

public class ApprovePendingUserRequestModel
{
    public string RegisterCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}