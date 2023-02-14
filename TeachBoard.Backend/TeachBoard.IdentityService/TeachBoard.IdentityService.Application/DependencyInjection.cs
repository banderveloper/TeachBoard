using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TeachBoard.IdentityService.Application.Services;
using TeachBoard.IdentityService.Application.Services.Background;

namespace TeachBoard.IdentityService.Application;

// DI of application layer to services
public static class DependencyInjection
{
    // builder.Services.AddApplication()
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddScoped<CookieProvider>();
        services.AddScoped<JwtProvider>();

        services.AddHostedService<PendingUsersCleaner>();
        
        return services;
    }
}