using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.Persistence;

namespace TeachBoard.MembersService.IntegrationTests;

public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryMembersTest");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            appContext.Database.EnsureCreated();
            
            AddTestGroups(appContext);
            AddTestStudents(appContext);
        });
    }

    private void AddTestGroups(ApplicationDbContext context)
    {
        context.Groups.AddRange(
            new Group
            {
                Id = 1,
                Name = "Test group 1"
            },
            new Group
            {
                Id = 2,
                Name = "Test group 2"
            },
            new Group
            {
                Id = 3,
                Name = "Test group 3"
            }
        );

        context.SaveChanges();
    }

    private void AddTestStudents(ApplicationDbContext context)
    {
        context.Students.AddRange(
            new Student
            {
                Id = 1,
                UserId = 1,
                GroupId = 1
            },
            new Student
            {
                Id = 2,
                UserId = 2,
                GroupId = 1
            },
            new Student
            {
                Id = 3,
                UserId = 3,
                GroupId = 2
            }
        );

        context.SaveChanges();
    }
}