using Microsoft.Extensions.DependencyInjection;
using TeachBoard.FileService.Application.Interfaces;
using TeachBoard.FileService.Application.Services;

namespace TeachBoard.FileService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IImageFileService, ImageFileService>();
        services.AddScoped<IFileService, Services.FileService>();

        return services;
    }
}