using Microsoft.EntityFrameworkCore;

namespace TeachBoard.MembersService.Persistence;

/// <summary>
/// Database initializer for creating db if is not exists
/// </summary>
public class DatabaseInitializer
{
    /// <summary>
    /// Create database and migrate if it is not exists.
    /// Recommended invokation during server starting
    /// </summary>
    /// <param name="context">EF database context</param>
    public static void Initialize(DbContext context)
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}