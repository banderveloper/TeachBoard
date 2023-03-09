namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;

// Command handling result, register code bound to created pending user
public class RegisterCodeModel
{
    public string RegisterCode { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}