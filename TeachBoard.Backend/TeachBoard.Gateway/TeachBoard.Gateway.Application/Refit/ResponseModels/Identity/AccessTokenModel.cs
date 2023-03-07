namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;

public class AccessTokenModel
{
    public string? AccessToken { get; set; }
    public int Expires { get; set; }
}