using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.Interfaces;

namespace TeachBoard.IdentityService.Persistence;

// DI of persistence layer to services
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // Get service configuration from services
        var scope = services.BuildServiceProvider().CreateScope();
        var dbConfig = scope.ServiceProvider.GetService<DatabaseConfiguration>();

        // register db context
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var filledConnectionString = string.Format(dbConfig.ConnectionString, dbConfig.User, dbConfig.Password);

            //Console.WriteLine("Filled conn string: " + filledConnectionString);
            
            //options.UseSqlite(connectionConfiguration!.Sqlite);
            options.UseNpgsql(filledConnectionString);
            
            // for optimizing read-only queries, disabling caching of entities
            // in ef select queries before update should be added .AsTracking method  
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        
        // bind db context interface to class
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetService<ApplicationDbContext>()!);

        return services;
    }
}