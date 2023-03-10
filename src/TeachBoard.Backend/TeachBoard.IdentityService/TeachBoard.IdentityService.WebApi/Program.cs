using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TeachBoard.IdentityService.Application;
using TeachBoard.IdentityService.Application.Converters;
using TeachBoard.IdentityService.Application.Mappings;
using TeachBoard.IdentityService.Domain.Enums;
using TeachBoard.IdentityService.Persistence;
using TeachBoard.IdentityService.WebApi;
using TeachBoard.IdentityService.WebApi.ActionResults;
using TeachBoard.IdentityService.WebApi.Middleware;
using TeachBoard.IdentityService.WebApi.Validation;

var builder = WebApplication.CreateBuilder(args);

// DI for custom configuration class
builder.Services.AddCustomConfiguration(builder.Configuration);

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
        options.JsonSerializerOptions.ReferenceHandler =ReferenceHandler.IgnoreCycles;
        
        
        // UserRole enum to string converter
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<UserRole>());

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

    config.SwaggerEndpoint("swagger/v1/swagger.json", "TeachBoard.IdentityService API");
});

// Activate "AllowAll" CORS policy
app.UseCors("AllowAll");

app.MapControllers();

app.Run();