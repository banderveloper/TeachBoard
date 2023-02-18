using Refit;
using TeachBoard.StudentAggregator.Clients;
using TeachBoard.StudentAggregator.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddRefitClient<IIdentityClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiUrls:Identity"]));

var app = builder.Build();

app.UseCustomExceptionHandler();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action}/{id?}");

app.Run();