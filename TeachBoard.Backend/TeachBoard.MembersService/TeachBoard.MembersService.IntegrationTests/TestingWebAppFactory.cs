using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.Domain.Enums;
using TeachBoard.MembersService.Persistence;

namespace TeachBoard.MembersService.IntegrationTests;

public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    private static bool _isFilled = false;

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

            appContext.Database.EnsureDeleted();
            appContext.Database.EnsureCreated();

            AddTestGroups(appContext);
            AddTestStudents(appContext);
            AddTestTeachers(appContext);
            AddTestFeedbacks(appContext);
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

    private void AddTestTeachers(ApplicationDbContext context)
    {
        context.Teachers.AddRange(
            new Teacher
            {
                Id = 1,
                UserId = 4,
            },
            new Teacher
            {
                Id = 2,
                UserId = 5
            });

        context.SaveChanges();
    }

    private void AddTestFeedbacks(ApplicationDbContext context)
    {
        context.Feedbacks.AddRange(
            new Feedback
            {
                Id = 1,
                StudentId = 1,
                TeacherId = 1,
                Rating = 8,
                Text = "Norm teacher",
                Direction = FeedbackDirection.StudentToTeacher
            },
            new Feedback
            {
                Id = 2,
                StudentId = 2,
                TeacherId = 2,
                Rating = 8,
                Text = "Norm teacher",
                Direction = FeedbackDirection.StudentToTeacher
            },
            new Feedback
            {
                Id = 3,
                StudentId = 1,
                TeacherId = 2,
                Rating = 2,
                Text = "Bad teacher",
                Direction = FeedbackDirection.StudentToTeacher
            },
            new Feedback
            {
                Id = 4,
                TeacherId = 1,
                StudentId = 1,
                Rating = 5,
                Text = "Not bad student",
                Direction = FeedbackDirection.TeacherToStudent
            },
            new Feedback
            {
                Id = 5,
                TeacherId = 1,
                StudentId = 2,
                Rating = 10,
                Text = "Perfect student",
                Direction = FeedbackDirection.TeacherToStudent
            },
            new Feedback
            {
                Id = 6,
                TeacherId = 2,
                StudentId = 1,
                Rating = 0,
                Text = "gg",
                Direction = FeedbackDirection.TeacherToStudent
            }
        );

        context.SaveChanges();
    }
}