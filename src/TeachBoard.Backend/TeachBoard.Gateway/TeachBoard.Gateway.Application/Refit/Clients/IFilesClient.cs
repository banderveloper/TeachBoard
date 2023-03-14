using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Refit;
using TeachBoard.Gateway.Application.Refit.RequestModels.Files;

namespace TeachBoard.Gateway.Application.Refit.Clients;

public interface IFilesClient
{
    [Multipart]
    [Post("/image/avatar/{userId}")]
    Task<ServiceTypedResponse<ImageUploadResult>> SetUserAvatar(int userId, [AliasAs("imageFile")] StreamPart stream);
    
}