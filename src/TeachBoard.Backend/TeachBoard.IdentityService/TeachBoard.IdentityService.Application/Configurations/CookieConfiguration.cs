namespace TeachBoard.IdentityService.Application.Configurations;

public class CookieConfiguration
{
    public string RefreshCookieName { get; set; } = string.Empty;
    public int RefreshCookieLifetimeHours { get; set; }
}