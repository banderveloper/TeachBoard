using System.Net;
using System.Text.Json;
using System.Reflection;
using Microsoft.Extensions.Options;
using TeachBoard.MembersService.Application;
using TeachBoard.MembersService.Persistence;
using TeachBoard.MembersService.WebApi.Middleware;
using TeachBoard.MembersService.Application.Mappings;
using TeachBoard.MembersService.Application.Configurations;
using TeachBoard.MembersService.Application.Converters;
using TeachBoard.MembersService.Domain.Enums;
using TeachBoard.MembersService.WebApi.ActionResults;
using TeachBoard.MembersService.WebApi.Validation;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Connection configuration registration
builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection("Database"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);

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
        
        // FeedbackDirection enum to string converter
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<FeedbackDirection>());
        
        // ErrorCode enum to snake_case string converter
        options.JsonSerializerOptions.Converters.Add(new SnakeCaseStringEnumConverter<ErrorCode>());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // adding validation error and 422 http to WebApiResponse while model state is not valid
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new WebApiResult
            {
                Error = new ValidationResultModel(context.ModelState),
                StatusCode = HttpStatusCode.UnprocessableEntity
            };

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

    config.SwaggerEndpoint("swagger/v1/swagger.json", "TeachBoard.MembersService API");
});

// Activate "AllowAll" CORS policy
app.UseCors("AllowAll");

app.MapControllers();

app.Run();