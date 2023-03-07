namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;

public class UserPresentationDataModel
{
    public int Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }

    public string? AvatarImagePath { get; set; }
}