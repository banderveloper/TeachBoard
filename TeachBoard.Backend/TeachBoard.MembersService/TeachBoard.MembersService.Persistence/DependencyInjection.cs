using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeachBoard.MembersService.Application.Configurations;
using TeachBoard.MembersService.Application.Interfaces;

namespace TeachBoard.MembersService.Persistence;

/// <summary>
/// Class containing extension method for injecting persistence layer to webapi project
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Extension method of ServiceCollection for injecting persistence layer 
    /// </summary>
    /// <param name="services">WebApi ServiceCollection</param>
    /// <returns>WebApi ServiceCollection with injected persistence layer deps</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // Get service configuration from services
        var scope = services.BuildServiceProvider().CreateScope();
        var connectionConfiguration = scope.ServiceProvider.GetService<ConnectionConfiguration>();

        // register db context
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionConfiguration.Sqlite);
        });
        
        // bind db context interface to class
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetService<ApplicationDbContext>());

        return services;
    }
}