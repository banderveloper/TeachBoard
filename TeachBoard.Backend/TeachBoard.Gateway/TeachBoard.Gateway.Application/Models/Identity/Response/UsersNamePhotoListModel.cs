namespace TeachBoard.Gateway.Application.Models.Identity.Response;

public class UsersNamePhotoListModel
{
    public IList<UserNamePhotoDto> Users { get; set; }
}

public class UserNamePhotoDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string AvatarImagePath { get; set; }
}