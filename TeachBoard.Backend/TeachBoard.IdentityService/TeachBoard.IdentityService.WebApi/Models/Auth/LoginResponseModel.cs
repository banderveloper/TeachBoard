namespace TeachBoard.IdentityService.WebApi.Models.Auth;

public class LoginResponseModel
{
    public string? AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}