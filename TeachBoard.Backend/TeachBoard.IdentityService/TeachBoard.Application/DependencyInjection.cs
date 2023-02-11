using Microsoft.Extensions.DependencyInjection;

namespace TeachBoard.Application;

// DI of application layer to services
public static class DependencyInjection
{
    // builder.Services.AddApplication()
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // ... inject mediatr and services

        return services;
    }
}