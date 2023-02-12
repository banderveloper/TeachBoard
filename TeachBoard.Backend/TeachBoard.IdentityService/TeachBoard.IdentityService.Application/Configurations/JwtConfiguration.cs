namespace TeachBoard.IdentityService.Application.Configurations;

public class JwtConfiguration
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int MinutesToExpiration { get; set; }
 
    public TimeSpan Expire => TimeSpan.FromMinutes(MinutesToExpiration);
}