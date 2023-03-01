namespace TeachBoard.Gateway.Application.Models.Identity.Response;

public class RegisterCodeResponseModel
{
    public string RegisterCode { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}