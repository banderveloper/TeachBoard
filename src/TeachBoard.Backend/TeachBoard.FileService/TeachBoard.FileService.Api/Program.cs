using System.Text.Json;
using Microsoft.Extensions.Options;
using TeachBoard.FileService.Api.Middleware;
using TeachBoard.FileService.Application;
using TeachBoard.FileService.Application.Configurations;
using TeachBoard.FileService.Application.Converters;
using TeachBoard.FileService.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<ImageApiConfiguration>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ImageApiConfiguration>>().Value);

builder.Services.Configure<FileApiConfiguration>(builder.Configuration.GetSection("S3"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<FileApiConfiguration>>().Value);

builder.Services.Configure<ConnectionConfiguration>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ConnectionConfiguration>>().Value);


// DI from another layers
builder.Services.AddApplication().AddPersistence();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // lowercase for json keys
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;

        // Error code enum to snake_case_string converter
        options.JsonSerializerOptions.Converters.Add(new SnakeCaseStringEnumConverter<ErrorCode>());
    });

// Database initialize if it is null
try
{
    var scope = builder.Services.BuildServiceProvider().CreateScope();
    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

    DatabaseInitializer.Initialize(context);
}
catch (Exception ex)
{
    // temporary
    Console.WriteLine(ex);
}

var app = builder.Build();

app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();