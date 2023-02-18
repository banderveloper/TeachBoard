using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Application.Features.Students.Common;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.WebApi.Models.Student;

namespace TeachBoard.MembersService.IntegrationTests;

public class StudentControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;

    public StudentControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task GetById_OnExistingId_ReturnsStudent()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync($"members/student/getbyid/1");
        response.EnsureSuccessStatusCode();
        
        var student = await response.Content.ReadFromJsonAsync<Student>();

        // Assert
        Assert.NotNull(student);
        Assert.Equal(1, student.UserId);
    }

    [Fact]
    public async Task GetById_OnWrongId_ReturnsNotFoundModel()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync($"members/student/getbyid/50000");
        var notFoundModel = await response.Content.ReadFromJsonAsync<NotFoundException>();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("student_not_found", notFoundModel?.Error);
        Assert.Equal("id", notFoundModel?.ReasonField);
    }

    [Fact]
    public async Task GetByGroupId_OnExistingGroupId_ReturnsStudents()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync($"members/student/getbygroupid/1");
        response.EnsureSuccessStatusCode();

        var studentsModel = await response.Content.ReadFromJsonAsync<StudentsListModel>();

        // Assert
        Assert.NotNull(studentsModel);
        Assert.Equal(2, studentsModel.Students.Count);

        foreach (var student in studentsModel.Students)
            Assert.Equal(1, student.GroupId);
    }

    [Fact]
    public async Task GetByGroupId_OnWrongGroupId_ReturnsNotFoundModel()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync($"members/student/getbygroupid/50000");
        var notFoundModel = await response.Content.ReadFromJsonAsync<NotFoundException>();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("students_not_found", notFoundModel?.Error);
        Assert.Equal("groupId", notFoundModel?.ReasonField);
    }

    [Fact]
    public async Task Create_OnCorrectRequestData_ReturnsStudent()
    {
        // Arrange
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "members/student/create");
        var requestBody = new CreateStudentRequestModel { UserId = 4, GroupId = 2 };

        postRequest.Content = JsonContent.Create(
            requestBody,
            MediaTypeHeaderValue.Parse("application/json")
        );

        // Act
        var response = await _client.SendAsync(postRequest);
        response.EnsureSuccessStatusCode();
        var student = await response.Content.ReadFromJsonAsync<Student>();

        // Assert
        Assert.NotNull(student);
        Assert.Equal(4, student.UserId);
        Assert.Equal(2, student.GroupId);
        Assert.True(student.Id > 0);
    }
    
    [Fact]
    public async Task Create_OnExistingUserId_ReturnsAlreadyExistsModel()
    {
        // Arrange
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "members/student/create");
        var requestBody = new CreateStudentRequestModel { UserId = 1, GroupId = 2 };

        postRequest.Content = JsonContent.Create(
            requestBody,
            MediaTypeHeaderValue.Parse("application/json")
        );

        // Act
        var response = await _client.SendAsync(postRequest);

        var alreadyExistsModel = await response.Content.ReadFromJsonAsync<AlreadyExistsException>();

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.NotNull(alreadyExistsModel);
        Assert.Equal("student_already_exists", alreadyExistsModel.Error);
        Assert.Equal("userId", alreadyExistsModel.ReasonField);
    }
    
    [Fact]
    public async Task Create_OnWrongGroupId_ReturnsNotFoundModel()
    {
        // Arrange
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "members/student/create");
        var requestBody = new CreateStudentRequestModel { UserId = 4, GroupId = 9000 };

        postRequest.Content = JsonContent.Create(
            requestBody,
            MediaTypeHeaderValue.Parse("application/json")
        );

        // Act
        var response = await _client.SendAsync(postRequest);

        var notFoundModel = await response.Content.ReadFromJsonAsync<NotFoundException>();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(notFoundModel);
        Assert.Equal("group_not_found", notFoundModel.Error);
        Assert.Equal("groupId", notFoundModel.ReasonField);
    }
    
    [Fact]
    public async Task Create_OnInvalidUserId_Returns422UnprocessableEntity()
    {
        // Arrange
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "members/student/create");
        var requestBody = new CreateStudentRequestModel { UserId = -100, GroupId = 2 };

        postRequest.Content = JsonContent.Create(
            requestBody,
            MediaTypeHeaderValue.Parse("application/json")
        );

        // Act
        var response = await _client.SendAsync(postRequest);

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }
}