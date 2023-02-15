using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TeachBoard.MembersService.Application;

/// <summary>
/// Class containing extension method for injecting application layer to webapi project
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Extension method of ServiceCollection for injecting application layer 
    /// </summary>
    /// <param name="services">WebApi ServiceCollection</param>
    /// <returns>WebApi ServiceCollection with injected application layer deps</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}