using Refit;
using TeachBoard.Gateway.Application.Refit.RequestModels.Identity;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;

namespace TeachBoard.Gateway.Application.Refit.Clients;

public interface IIdentityClient
{
    /// <summary>
    /// Login and get jwt tokens
    /// </summary>
    /// <param name="model">Model with username and password</param>
    /// <returns>Access token, his expiration time and refresh token at cookie</returns>
    [Post("/auth/login")]
    Task<ApiResponse<ServiceTypedResponse<AccessTokenResponseModel>>> Login(LoginRequestModel model);

    /// <summary>
    /// Refresh session and get new tokens
    /// </summary>
    /// <param name="refreshCookie">Refresh cookie in format: cookie_name=cookie_value</param>
    /// <returns>Model with new access token</returns>
    [Put("/auth/refresh")]
    Task<ApiResponse<ServiceTypedResponse<AccessTokenResponseModel>>> Refresh([Header("Cookie")] string refreshCookie);
    
    /// <summary>
    /// Logout / end session
    /// </summary>
    /// <param name="refreshCookie">Refresh cookie in format cookie_name=cookie_value</param>
    [Delete("/auth/logout")]
    Task<ApiResponse<ServiceTypedResponse<object>>> Logout([Header("Cookie")] string refreshCookie);
    
    /// <summary>
    /// Approve pending user by register code
    /// </summary>
    /// <param name="model">Pending register code, new login and password</param>
    /// <returns>Created user from pending user</returns>
    [Post("/user/pending/approve")]
    Task<ServiceTypedResponse<User>> ApprovePendingUser(ApprovePendingUserRequestModel model);
}