namespace TeachBoard.Persistence;

// db checker and initializer at server start
// invokes at main
public class DatabaseInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
    }
}