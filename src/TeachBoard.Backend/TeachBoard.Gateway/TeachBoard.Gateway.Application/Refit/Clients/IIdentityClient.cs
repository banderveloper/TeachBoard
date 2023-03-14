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
    Task<ApiResponse<ServiceTypedResponse<AccessTokenModel>>> Login(LoginRequestModel model);

    /// <summary>
    /// Refresh session and get new tokens
    /// </summary>
    /// <param name="refreshCookie">Refresh cookie in format: cookie_name=cookie_value</param>
    /// <returns>Model with new access token</returns>
    [Put("/auth/refresh")]
    Task<ApiResponse<ServiceTypedResponse<AccessTokenModel>>> Refresh([Header("Cookie")] string refreshCookie);

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

    /// <summary>
    /// Get list of users presentation models (id, name, avatar)
    /// </summary>
    /// <param name="userIds">Users ids</param>
    /// <returns>List of users presentation models</returns>
    [Get("/user/presentation")]
    Task<ServiceTypedResponse<IList<UserPresentationDataModel>>> GetUserPresentationDataModels(
        [Query(CollectionFormat.Multi)] List<int> userIds);

    /// <summary>
    /// Get user public data (without password)
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>User public data (without password)</returns>
    [Get("/user/{userId}")]
    Task<ServiceTypedResponse<UserPublicData?>> GetUserPublicData(int userId);

    /// <summary>
    /// Create pending user
    /// </summary>
    /// <param name="model">New pending user data</param>
    /// <returns>Registration code and expiration time</returns>
    [Post("/user/pending")]
    Task<ServiceTypedResponse<RegisterCodeModel>> CreatePendingUser([Body] CreatePendingUserRequestModel model);

    /// <summary>
    /// Get list of users presentation models (id, name, avatar) by partial name
    /// </summary>
    /// <param name="partialName">Part of the name</param>
    /// <returns>List of users presentation models</returns>
    [Get("/user/presentation/{partialName}")]
    Task<ServiceTypedResponse<IList<UserPresentationDataModel>>> GetUserPresentationDataModelsByPartialName(
        string partialName);

    /// <summary>
    /// Update user public data
    /// </summary>
    /// <param name="model">New user public data</param>
    /// <returns>Updated user public data</returns>
    [Put("/user")]
    Task<ServiceTypedResponse<UserPublicData>> UpdateUser([Body] UpdateUserRequestModel model);

    [Put("/user/avatar")]
    Task<ServiceTypedResponse<UserPublicData>> UpdateUserAvatar([Body] UpdateUserAvatarRequestModel model);
}