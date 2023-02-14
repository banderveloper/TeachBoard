using Microsoft.Extensions.Options;
using TeachBoard.IdentityService.Application.Configurations;

namespace TeachBoard.IdentityService.WebApi;

// Custom configuration injection extension class
public static class ConfigurationDependencyInjection
{
    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        // JWT configuration registration
        services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<JwtConfiguration>>().Value);

        // Connection configuration registration
        services.Configure<ConnectionConfiguration>(configuration.GetSection("ConnectionStrings"));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<ConnectionConfiguration>>().Value);

        // Cookie configuration registration
        services.Configure<CookieConfiguration>(configuration.GetSection("Cookie"));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<CookieConfiguration>>().Value);

        // Pending user configuration registration
        services.Configure<PendingUserConfiguration>(configuration.GetSection("PendingUser"));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<PendingUserConfiguration>>().Value);

        return services;
    }
}