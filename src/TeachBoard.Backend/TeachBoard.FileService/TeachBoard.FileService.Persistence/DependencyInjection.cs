using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeachBoard.FileService.Application.Configurations;
using TeachBoard.FileService.Application.Interfaces;

namespace TeachBoard.FileService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // Get service configuration from services
        var scope = services.BuildServiceProvider().CreateScope();
        var dbConfig = scope.ServiceProvider.GetService<DatabaseConfiguration>();

        var filledConnectionString = string.Format(dbConfig.ConnectionString, dbConfig.User, dbConfig.Password);
        
        // register db context
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(filledConnectionString);
                
            // for optimizing read-only queries, disabling caching of entities
            // in ef select queries before update should be added .AsTracking method  
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        // bind db context interface to class
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetService<ApplicationDbContext>());

        return services;
    }
}