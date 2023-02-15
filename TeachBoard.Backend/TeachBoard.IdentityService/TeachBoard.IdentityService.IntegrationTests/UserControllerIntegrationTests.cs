using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TeachBoard.IdentityService.WebApi.Models.User;
using Xunit.Abstractions;

namespace TeachBoard.IdentityService.IntegrationTests;

public class UserControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;

    public UserControllerIntegrationTests(TestingWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreatePendingUser_ReturnsRegisterCodeOnSuccess()
    {
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/identity/users/pending/create");
        _testOutputHelper.WriteLine(postRequest.RequestUri?.ToString());
        
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
        
        // todo
    }

    [Fact]
    public async Task GetUserById_Returns404OnWrongId()
    {
        var response = await _client.GetAsync($"/identity/users/getbyid/500000000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // [Fact]
    // public async Task GetUserById_ReturnsCorrectUser()
    // {
    //     var response = await _client.GetAsync($"identity/users/getbyid/1");
    //
    //     response.EnsureSuccessStatusCode();
    // }
}