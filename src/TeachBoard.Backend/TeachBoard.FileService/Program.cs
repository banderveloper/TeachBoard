using Microsoft.Extensions.Options;
using TeachBoard.FileService.Configurations;
using TeachBoard.FileService.Interfaces;
using TeachBoard.FileService.Middleware;
using TeachBoard.FileService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("Cloudinary_").AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddScoped<IImageFileService, ImageFileService>();

builder.Services.Configure<ImageApiConfiguration>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ImageApiConfiguration>>().Value);

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine("CLOUDINARY CLOUD NAME: " + builder.Configuration["Cloudinary:CloudName"]);
    await next.Invoke();
});

app.UseCustomExceptionHandler();

app.MapControllers();

Console.WriteLine("CONFIG CLOUDINARY: " + builder.Configuration["Cloudinary:CloudName"]);
Console.WriteLine("ENVIRONMENT: " + builder.Environment);

app.Run();