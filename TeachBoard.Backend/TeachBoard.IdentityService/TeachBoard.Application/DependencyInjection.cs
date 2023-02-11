using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TeachBoard.Application;

// DI of application layer to services
public static class DependencyInjection
{
    // builder.Services.AddApplication()
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        return services;
    }
}