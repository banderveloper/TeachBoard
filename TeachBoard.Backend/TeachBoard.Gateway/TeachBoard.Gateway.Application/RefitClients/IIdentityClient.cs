using Refit;
using TeachBoard.Gateway.Application.Models.Identity;
using TeachBoard.Gateway.Application.Models.Identity.Request;
using TeachBoard.Gateway.Application.Models.Identity.Response;
using TeachBoard.Gateway.Domain.Enums;

namespace TeachBoard.Gateway.Application.RefitClients;

public interface IIdentityClient
{
    /// <summary>
    /// Get pending user role by register code
    /// </summary>
    /// <param name="registerCode">Registed code of pending user given after creating pending</param>
    /// <returns>Pending user role</returns>
    [Get("/users/pending/getRoleByCode/{registerCode}")]
    Task<UserRole> GetPendingUserRoleByRegisterCode(string registerCode);
    
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
    /// <param name="ids">List of users ids</param>
    /// <returns>Model with list of users dtos with names and photos</returns>
    [Get("/users/getNamesPhotosByIds")]
    Task<UsersNamePhotoListModel> GetUserNamesPhotosByIds([Body]List<int> ids);

    /// <summary>
    /// Get user by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>User public data (without id and password)</returns>
    [Get("/users/getById/{userId}")]
    Task<UserPublicDataModel> GetUserById(int userId);
}