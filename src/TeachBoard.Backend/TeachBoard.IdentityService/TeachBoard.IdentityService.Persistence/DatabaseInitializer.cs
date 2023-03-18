using Microsoft.EntityFrameworkCore;

namespace TeachBoard.IdentityService.Persistence;

// db checker and initializer at server start
// invokes at main
public class DatabaseInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        var isCreated = context.Database.EnsureCreated();
        Console.WriteLine("IS DB CREATED: " + isCreated);
        //context.Database.Migrate();
    }
}