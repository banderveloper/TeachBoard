using Refit;
using TeachBoard.StudentAggregator.Domain.Enums;
using TeachBoard.StudentAggregator.Models.Identity;

namespace TeachBoard.StudentAggregator.Clients;

public interface IIdentityClient
{
    [Get("/users/pending/getrolebycode/{registerCode}")]
    Task<Role> GetPendingUserRole(string registerCode);

    [Post("/users/pending/approve")]
    Task ApprovePendingUser(ApprovePendingUserModel model);
}