using Microsoft.Extensions.Options;
using TeachBoard.FileService.Configurations;
using TeachBoard.FileService.Interfaces;
using TeachBoard.FileService.Middleware;
using TeachBoard.FileService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddScoped<IImageFileService, ImageFileService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.Configure<ImageApiConfiguration>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ImageApiConfiguration>>().Value);

builder.Services.Configure<FileApiConfiguration>(builder.Configuration.GetSection("S3"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<FileApiConfiguration>>().Value);

var app = builder.Build();

app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();