namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;

public class AccessTokenResponseModel
{
    public string? AccessToken { get; set; }
    public int Expires { get; set; }
}