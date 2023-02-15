using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TeachBoard.IdentityService.Application.CQRS.Commands.CreatePendingUser;
using TeachBoard.IdentityService.Domain.Entities;
using TeachBoard.IdentityService.WebApi.Models.User;
using Xunit.Abstractions;

namespace TeachBoard.IdentityService.IntegrationTests;

public class UserControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;

    public UserControllerIntegrationTests(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Test: Create pending user and get register code from body
    /// Expected: 200 OK and register code
    /// </summary>
    [Fact]
    public async Task CreatePendingUser_ReturnsRegisterCodeOnSuccess()
    {
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/identity/users/pending/create");

        var createPendingUserModel = new CreatePendingUserRequestModel()
        {
            FirstName = "Nikita",
            Email = "idkasdasd@gmail.com"
        };

        postRequest.Content = JsonContent.Create(
            createPendingUserModel,
            MediaTypeHeaderValue.Parse("application/json")
        );

        var httpResponse = await _client.SendAsync(postRequest);

        httpResponse.EnsureSuccessStatusCode();

        var registerCodeResponse = await httpResponse.Content.ReadFromJsonAsync<RegisterCodeModel>();
        
        Assert.NotNull(registerCodeResponse);
        Assert.True(registerCodeResponse.RegisterCode.Length == 8);
        Assert.True(registerCodeResponse.ExpiresAt > DateTime.Now);
    }

    /// <summary>
    /// Test: Get HTTP404 trying get user with unreal id
    /// Expected: 404
    /// </summary>
    [Fact]
    public async Task GetUserById_Returns404OnWrongId()
    {
        var response = await _client.GetAsync($"/identity/users/getbyid/500000000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// Try get existing user with id 1
    /// Expected: 200OK and username user1
    /// </summary>
    [Fact]
    public async Task GetUserById_ReturnsCorrectUser()
    {
        var response = await _client.GetAsync($"identity/users/getbyid/1");
        
        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<User>();
        
        Assert.NotNull(user);
        Assert.Equal("test1", user.UserName);
    }
}