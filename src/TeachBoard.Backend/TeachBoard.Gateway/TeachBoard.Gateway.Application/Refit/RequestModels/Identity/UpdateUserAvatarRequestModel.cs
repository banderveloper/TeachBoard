namespace TeachBoard.Gateway.Application.Refit.RequestModels.Identity;

public class UpdateUserAvatarRequestModel
{
    public int UserId { get; set; }
    public string? AvatarImagePath { get; set; }
}