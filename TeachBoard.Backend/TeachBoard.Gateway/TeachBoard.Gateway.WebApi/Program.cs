using System.Text.Json;
using Refit;
using TeachBoard.Gateway.Application.RefitClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // lowercase for json keys
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

// Register refit IdentityClient
builder.Services.AddRefitClient<IIdentityClient>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.Configuration["ApiAddresses:Identity"]));

// Register refit IdentityClient
builder.Services.AddRefitClient<IMembersClient>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.Configuration["ApiAddresses:Members"]));

var app = builder.Build();

app.MapControllers();



app.Run();