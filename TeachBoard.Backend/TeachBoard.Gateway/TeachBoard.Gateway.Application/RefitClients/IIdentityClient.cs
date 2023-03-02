using Refit;
using TeachBoard.Gateway.Application.Models.Identity;
using TeachBoard.Gateway.Application.Models.Identity.Request;
using TeachBoard.Gateway.Application.Models.Identity.Response;

namespace TeachBoard.Gateway.Application.RefitClients;

public interface IIdentityClient
{
    /// <summary>
    /// Approve pending user by register code
    /// </summary>
    /// <param name="model">Model containing pending register code, new login and password</param>
    /// <returns>Created user from pending user</returns>
    [Post("/users/pending/approve")]
    Task<User> ApprovePendingUser(ApprovePendingUserRequestModel model);

    /// <summary>
    /// Get users names and photos by users ids
    /// </summary>
    /// <param name="userId">List of users ids</param>
    /// <returns>Model with list of users dtos with names and photos</returns>
    [Get("/users/names-photos")]
    Task<UsersNamePhotoListModel> GetUserNamesPhotosByIds([Query(CollectionFormat.Multi)] List<int> userId);

    /// <summary>
    /// Get user by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>User public data (without id and password)</returns>
    [Get("/users/{userId}")]
    Task<UserPublicDataModel> GetUserById(int userId);

    /// <summary>
    /// Login and get jwt tokens
    /// </summary>
    /// <param name="model">Model with username and password</param>
    /// <returns>Access token, his expiration time and refresh token at cookie</returns>
    [Post("/auth/login")]
    Task<ApiResponse<AuthTokenResponseModel>> Login(LoginRequestModel model);

    /// <summary>
    /// Refresh session and get new tokens (refresh)
    /// </summary>
    /// <param name="refreshCookie">Refresh cookie in format: cookie_name=cookie_value</param>
    /// <returns>Model with new access token</returns>
    [Put("/auth/refresh")]
    Task<ApiResponse<AuthTokenResponseModel>> Refresh([Header("Cookie")] string refreshCookie);

    /// <summary>
    /// Logout / end session
    /// </summary>
    /// <param name="refreshCookie">Refresh cookie in format cookie_name=cookie_value</param>
    [Delete("/auth/logout")]
    Task<IApiResponse> Logout([Header("Cookie")] string refreshCookie);

    /// <summary>
    /// Create pending user (by admin or director)
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Model with register code and expiration time</returns>
    [Post("/users/pending")]
    Task<RegisterCodeResponseModel> CreatePendingUser([Body] CreatePendingUserRequestModel model);
}