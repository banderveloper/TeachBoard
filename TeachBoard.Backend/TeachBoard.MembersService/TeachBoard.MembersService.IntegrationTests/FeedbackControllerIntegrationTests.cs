using System.Net.Http.Headers;
using System.Net.Http.Json;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.Domain.Enums;
using TeachBoard.MembersService.WebApi.Models.Feedback;

namespace TeachBoard.MembersService.IntegrationTests;

public class FeedbackControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;

    public FeedbackControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task CreateStudentToTeacherFeedback_OnSuccess_ReturnsFeedback()
    {
        // Arrange
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "members/feedback/create/student/teacher");
        var requestBody = new CreateFeedbackRequestModel
            { StudentId = 1, TeacherId = 1, Rating = 5, Text = "test text" };

        postRequest.Content = JsonContent.Create(
            requestBody,
            MediaTypeHeaderValue.Parse("application/json")
        );

        // Act
        var response = await _client.SendAsync(postRequest);
        response.EnsureSuccessStatusCode();
        var feedback = await response.Content.ReadFromJsonAsync<Feedback>();

        // Assert
        Assert.NotNull(feedback);
        Assert.Equal(FeedbackDirection.StudentToTeacher, feedback.Direction);
        Assert.Equal("test text", feedback.Text);
        Assert.Equal(5, feedback.Rating);
        Assert.Equal(1, feedback.TeacherId);
        Assert.Equal(1, feedback.StudentId);
        Assert.True(feedback.Id > 0);
    }
}