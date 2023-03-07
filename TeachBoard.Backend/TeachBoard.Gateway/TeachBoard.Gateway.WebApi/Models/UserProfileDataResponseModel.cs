using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Members;

namespace TeachBoard.Gateway.WebApi.Models;

public class UserProfileDataResponseModel
{
    public UserPublicData? User { get; set; }
    public Group? Group { get; set; }
}