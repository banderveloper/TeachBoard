using System.Net;
using System.Net.Http.Json;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.IntegrationTests;

public class StudentControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;

    public StudentControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task GetById_OnExistingId_ReturnsStudent()
    {
        var response = await _client.GetAsync($"members/students/getbyid/1");

        response.EnsureSuccessStatusCode();

        var student = await response.Content.ReadFromJsonAsync<Student>();
        
        Assert.NotNull(student);
        Assert.Equal(1, student.UserId);
    }

    [Fact]
    public async Task GetById_OnWrongId_ReturnsNotFoundModel()
    {
        var response = await _client.GetAsync($"members/students/getbyid/50000");
        var notFoundModel = await response.Content.ReadFromJsonAsync<NotFoundException>();
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("student_not_found", notFoundModel?.Error);
        Assert.Equal("id", notFoundModel?.ReasonField);
    }

    [Fact]
    public async Task GetByGroupId_OnExistingGroupId_ReturnsStudents()
    {
        var response = await _client.GetAsync($"members/students/getbygroupid/1");

        response.EnsureSuccessStatusCode();

        var studentsModel = await response.Content.ReadFromJsonAsync<StudentsListModel>();
        
        Assert.NotNull(studentsModel);
        Assert.Equal(2, studentsModel.Students.Count);

        foreach (var student in studentsModel.Students)
        {
            Assert.Equal(1, student.GroupId);
        }
    }
    
    [Fact]
    public async Task GetByGroupId_OnWrongGroupId_ReturnsNotFoundModel()
    {
        var response = await _client.GetAsync($"members/students/getbygroupid/50000");
        var notFoundModel = await response.Content.ReadFromJsonAsync<NotFoundException>();
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("students_not_found", notFoundModel?.Error);
        Assert.Equal("groupId", notFoundModel?.ReasonField);
    }
}