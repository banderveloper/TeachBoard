using System.Net.Mime;
using System.Reflection;
using Refit;
using System.Text.Json;
using TeachBoard.Gateway.WebApi.Middleware;
using TeachBoard.Gateway.Application.RefitClients;
using TeachBoard.Gateway.WebApi.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // lowercase for json keys
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    }).ConfigureApiBehaviorOptions(options =>
    {
        // custom validation error response
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new ValidationFailedResult(context.ModelState);
            result.ContentTypes.Add(MediaTypeNames.Application.Json);

            return result;
        };
    });

// Register refit IdentityClient
builder.Services.AddRefitClient<IIdentityClient>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.Configuration["ApiAddresses:Identity"]));

// Register refit MembersClient
builder.Services.AddRefitClient<IMembersClient>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.Configuration["ApiAddresses:Members"]));

// Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(config =>
{
    // xml comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseCustomExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    // get to swagger UI using root uri
    config.RoutePrefix = string.Empty;

    config.SwaggerEndpoint("swagger/v1/swagger.json", "TeachBoard.Gateway");
});

app.MapControllers();

app.Run();