using Microsoft.EntityFrameworkCore;

namespace TeachBoard.EducationService.Persistence;

public class DatabaseInitializer
{
    public static void Initialize(DbContext context)
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}