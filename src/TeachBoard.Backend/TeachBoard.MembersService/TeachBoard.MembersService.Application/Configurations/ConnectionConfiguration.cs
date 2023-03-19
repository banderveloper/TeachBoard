namespace TeachBoard.MembersService.Application.Configurations;

/// <summary>
/// Container of connection strings from appsettings
/// </summary>
public class ConnectionConfiguration
{
    public string Sqlite { get; set; } = string.Empty;
    public string Postgres { get; set; } = string.Empty;
}