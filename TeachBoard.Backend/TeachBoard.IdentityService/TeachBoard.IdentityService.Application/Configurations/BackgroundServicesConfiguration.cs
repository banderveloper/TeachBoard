namespace TeachBoard.IdentityService.Application.Configurations;

public class BackgroundServicesConfiguration
{
    public int PendingUsersCleanIntervalHours { get; set; }
    public int RefreshSessionsCleanIntervalHours { get; set; }
}