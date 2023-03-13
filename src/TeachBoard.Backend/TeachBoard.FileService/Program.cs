using Microsoft.Extensions.Options;
using TeachBoard.FileService.Configurations;
using TeachBoard.FileService.Interfaces;
using TeachBoard.FileService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IImageFileService, ImageFileService>();

builder.Services.Configure<ImageApiConfiguration>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ImageApiConfiguration>>().Value);

var app = builder.Build();

app.MapControllers();

app.Run();