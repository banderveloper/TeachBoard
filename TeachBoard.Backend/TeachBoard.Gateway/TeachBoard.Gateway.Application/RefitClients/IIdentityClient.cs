using Refit;
using TeachBoard.Gateway.Application.Models.Identity;
using TeachBoard.Gateway.Domain.Enums;

namespace TeachBoard.Gateway.Application.RefitClients;

public interface IIdentityClient
{
    /// <summary>
    /// Get pending user role by register code
    /// </summary>
    /// <param name="registerCode">Registed code of pending user given after creating pending</param>
    /// <returns>Pending user role</returns>
    [Get("/users/pending/getrolebycode/{registerCode}")]
    Task<UserRole> GetPendingUserRoleByRegisterCode(string registerCode);

    
    /// <summary>
    /// Approve pending user by register code
    /// </summary>
    /// <param name="model">Model containing pending register code, new login and password</param>
    /// <returns>Created user from pending user</returns>
    [Post("/users/pending/approve")]
    Task<IdentityUserResponseModel> ApprovePendingUser(ApprovePendingUserTransferModel model);
}