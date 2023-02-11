using Microsoft.Extensions.DependencyInjection;

namespace TeachBoard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // ... inject mediatr and services

        return services;
    }
}