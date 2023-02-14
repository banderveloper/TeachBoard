using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TeachBoard.IdentityService.Application;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.Mappings;
using TeachBoard.IdentityService.Persistence;
using TeachBoard.IdentityService.WebApi.Middleware;
using TeachBoard.IdentityService.WebApi.Models.Validation;

var builder = WebApplication.CreateBuilder();

// JWT configuration registration
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<JwtConfiguration>>().Value);

// Connection configuration registration
builder.Services.Configure<ConnectionConfiguration>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ConnectionConfiguration>>().Value);

// Cookie configuration registration
builder.Services.Configure<CookieConfiguration>(builder.Configuration.GetSection("Cookie"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<CookieConfiguration>>().Value);

// DI from another layers
builder.Services.AddApplication().AddPersistence();

// CORS-policies for browser access
builder.Services.AddCors(options =>
{
    // All clients (temporary)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

// Controller and JSON configs registration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // lowercase for json keys
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // custom validation error response
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new ValidationFailedResult(context.ModelState);
            result.ContentTypes.Add(MediaTypeNames.Application.Json);

            return result;
        };
    });

// Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(config =>
{
    // xml comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

// Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(AssemblyMappingProfile).Assembly));
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

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    // get to swagger UI using root uri
    config.RoutePrefix = string.Empty;

    config.SwaggerEndpoint("swagger/v1/swagger.json", "TeachBoard.IdentityService API");
});

// Activate "AllowAll" CORS policy
app.UseCors("AllowAll");

app.MapControllers();

app.Run();