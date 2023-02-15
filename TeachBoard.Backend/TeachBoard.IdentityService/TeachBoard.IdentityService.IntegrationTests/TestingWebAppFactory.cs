using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeachBoard.IdentityService.Domain.Entities;
using TeachBoard.IdentityService.Domain.Enums;
using TeachBoard.IdentityService.Persistence;

namespace TeachBoard.IdentityService.IntegrationTests;

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
                options.UseInMemoryDatabase("InMemoryUsersTest");
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            appContext.Database.EnsureCreated();
                
            AddTestUsers(appContext);
            AddTestPendingUsers(appContext);
        });
    }

    private static void AddTestUsers(ApplicationDbContext context)
    {
        context.Users.AddRange(
            new User
            {
                Id = 1,
                UserName = "test1",
                FirstName = "TestFirstName1",
                LastName = "TestLastName1",
                Patronymic = "TestPatronymic1",
                Email = "test1@gmail.com",
                Role = Role.Student,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                HomeAddress = "home1",
                PasswordHash = "password1",
                PhoneNumber = "phone1",
                DateOfBirth = DateTime.Now
            },
            new User
            {
                Id = 2,
                UserName = "test2",
                FirstName = "TestFirstName2",
                LastName = "TestLastName2",
                Patronymic = "TestPatronymic2",
                Email = "test2@gmail.com",
                Role = Role.Student,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                HomeAddress = "home2",
                PasswordHash = "password2",
                PhoneNumber = "phone2",
                DateOfBirth = DateTime.Now
            }
        );
        context.SaveChanges();
    }

    private static void AddTestPendingUsers(ApplicationDbContext context)
    {
        context.PendingUsers.Add(new PendingUser
        {
            Id = 1,
            FirstName = "TestFirstName1",
            LastName = "TestLastName1",
            Patronymic = "TestPatronymic1",
            Email = "test1@gmail.com",
            Role = Role.Student,
            HomeAddress = "home1",
            PhoneNumber = "phone1",
            DateOfBirth = DateTime.Now,
            RegisterCode = "qwerty",
            ExpiresAt = new DateTime(2050, 5, 1)
        });
        // expired user
        context.PendingUsers.Add(new PendingUser
        {
            Id = 2,
            FirstName = "TestFirstName2",
            LastName = "TestLastName2",
            Patronymic = "TestPatronymic2",
            Email = "test2@gmail.com",
            Role = Role.Student,
            HomeAddress = "home2",
            PhoneNumber = "phone2",
            DateOfBirth = DateTime.Now,
            RegisterCode = "expire",
            ExpiresAt = new DateTime(2010, 5, 1)
        });
        
        context.SaveChanges();
    }
}