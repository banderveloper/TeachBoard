using Microsoft.AspNetCore.Http;
using Refit;

namespace TeachBoard.Gateway.Application.Refit.RequestModels.Files;

public class SetUserAvatarRequestModel
{
    public int UserId { get; set; }
    public IFormFile ImageFile { get; set; }
}