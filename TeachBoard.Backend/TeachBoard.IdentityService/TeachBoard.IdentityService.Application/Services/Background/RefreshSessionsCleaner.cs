using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.Interfaces;

namespace TeachBoard.IdentityService.Application.Services.Background;

public class RefreshSessionsCleaner : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly BackgroundServicesConfiguration _backgroundServicesConfiguration;
    private readonly CookieConfiguration _cookieConfiguration;
    private readonly ILogger<PendingUsersCleaner> _logger;

    public RefreshSessionsCleaner(BackgroundServicesConfiguration backgroundServicesConfiguration,
        ILogger<PendingUsersCleaner> logger, IServiceProvider provider, CookieConfiguration cookieConfiguration)
    {
        _backgroundServicesConfiguration = backgroundServicesConfiguration;
        _logger = logger;
        _provider = provider;
        _cookieConfiguration = cookieConfiguration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        var context = scope.ServiceProvider.GetService<IApplicationDbContext>()!;

        while (!stoppingToken.IsCancellationRequested)
        {
            // Console.WriteLine("Cleaned!");
            await RemoveExpiredRefreshSessions(context, stoppingToken);

            await Task.Delay(TimeSpan.FromHours(_backgroundServicesConfiguration.RefreshSessionsCleanIntervalHours),
                stoppingToken);
        }
    }

    private async Task RemoveExpiredRefreshSessions(IApplicationDbContext context, CancellationToken stoppingToken)
    {
        // Get expired sessions through the UPDATED datetime and refresh lifetime
        var expiredSessions = await context.RefreshSessions
            .Where(pu => pu.UpdatedAt.AddHours(_cookieConfiguration.RefreshCookieLifetimeHours) < DateTime.Now)
            .ToListAsync(cancellationToken: stoppingToken);

        if (expiredSessions.Count > 0)
        {
            _logger.LogInformation($"Removed {expiredSessions.Count} expired refresh sessions");
            context.RefreshSessions.RemoveRange(expiredSessions);
            await context.SaveChangesAsync(stoppingToken);
        }
    }
}