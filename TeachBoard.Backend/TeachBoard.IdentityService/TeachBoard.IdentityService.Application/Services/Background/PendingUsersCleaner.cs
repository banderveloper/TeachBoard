using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.Interfaces;

namespace TeachBoard.IdentityService.Application.Services.Background;

public class PendingUsersCleaner : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly BackgroundServicesConfiguration _configuration;
    private readonly ILogger<PendingUsersCleaner> _logger;

    public PendingUsersCleaner(BackgroundServicesConfiguration configuration,
        ILogger<PendingUsersCleaner> logger, IServiceProvider provider)
    {
        _configuration = configuration;
        _logger = logger;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        var context = scope.ServiceProvider.GetService<IApplicationDbContext>()!;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            // Console.WriteLine("Cleaned!");
            await RemoveExpiredPendingUsers(context, stoppingToken);

            await Task.Delay(TimeSpan.FromHours(_configuration.PendingUsersCleanIntervalHours), stoppingToken);
        }
    }

    private async Task RemoveExpiredPendingUsers(IApplicationDbContext context, CancellationToken stoppingToken)
    {
        var expiredUsers = await context.PendingUsers
            .Where(pu => pu.ExpiresAt < DateTime.Now)
            .ToListAsync(cancellationToken: stoppingToken);

        if (expiredUsers.Count > 0)
        {
            _logger.LogInformation($"Removed {expiredUsers.Count} expired pending users");
            context.PendingUsers.RemoveRange(expiredUsers);
            await context.SaveChangesAsync(stoppingToken);
        }
    }
}