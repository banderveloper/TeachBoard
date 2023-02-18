namespace TeachBoard.StudentAggregator.Models.Identity;

public class ApprovePendingUserModel
{
    public string RegisterCode { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
}