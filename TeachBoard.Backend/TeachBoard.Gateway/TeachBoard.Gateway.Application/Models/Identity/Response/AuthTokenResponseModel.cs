namespace TeachBoard.Gateway.Application.Models.Identity.Response;

public class AuthTokenResponseModel
{
    public string AccessToken { get; set; }
    public int Expires { get; set; }
}