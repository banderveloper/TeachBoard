namespace TeachBoard.IdentityService.WebApi.Models.Auth;

public class AccessTokenResponseModel
{
    public string? AccessToken { get; set; }
    public int Expires { get; set; }
}