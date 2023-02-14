namespace TeachBoard.IdentityService.WebApi.Models.Auth;

public class AccessTokenModel
{
    public string? AccessToken { get; set; }
    public TimeSpan Expires { get; set; }
}