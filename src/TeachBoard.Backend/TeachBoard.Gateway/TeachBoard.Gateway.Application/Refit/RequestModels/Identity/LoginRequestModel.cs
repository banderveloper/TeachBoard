namespace TeachBoard.Gateway.Application.Refit.RequestModels.Identity;

public class LoginRequestModel
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}