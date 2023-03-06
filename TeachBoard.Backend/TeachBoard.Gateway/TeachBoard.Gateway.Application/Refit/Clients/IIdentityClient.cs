using Refit;
using TeachBoard.Gateway.Application.Refit.RequestModels.Identity;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;

namespace TeachBoard.Gateway.Application.Refit.Clients;

public interface IIdentityClient
{
    [Post("/auth/login")]
    Task<ApiResponse<ServiceTypedResponse<AccessTokenResponseModel>>> Login(LoginRequestModel model);
}