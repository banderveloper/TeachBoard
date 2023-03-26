using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TeachBoard.EducationService.Application;
using TeachBoard.EducationService.Application.Configurations;
using TeachBoard.EducationService.Application.Converters;
using TeachBoard.EducationService.Application.Mappings;
using TeachBoard.EducationService.Domain.Enums;
using TeachBoard.EducationService.Persistence;
using TeachBoard.EducationService.WebApi.ActionResults;
using TeachBoard.EducationService.WebApi.Middleware;
using TeachBoard.EducationService.WebApi.Validation;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Connection configuration registration
builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection("Database"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);

// Lesson configuration registration
builder.Services.Configure<LessonConfiguration>(builder.Configuration.GetSection("Lesson"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<LessonConfiguration>>().Value);

// DI from another layers
builder.Services.AddApplication().AddPersistence();

// Controller and JSON configs registration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // lowercase for json keys
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        
        // models enums to string converter
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<AttendanceStatus>());
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<StudentExaminationStatus>());
        
        // Error code enum to snake_case_string converter
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

    config.SwaggerEndpoint("swagger/v1/swagger.json", "TeachBoard.Education API");
});

app.MapControllers();

app.Run();