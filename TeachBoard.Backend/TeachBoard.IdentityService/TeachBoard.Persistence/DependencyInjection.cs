﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeachBoard.Application.Configurations;

namespace TeachBoard.Persistence;

// DI of persistence layer to services
public static class DependencyInjection
{
    // builder.Services.AddPersistence()
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

        return services;
    }
}