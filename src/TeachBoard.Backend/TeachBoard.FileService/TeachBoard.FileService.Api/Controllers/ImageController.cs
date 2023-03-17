using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Api.Models;
using TeachBoard.FileService.Application.Interfaces;

namespace TeachBoard.FileService.Api.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    private readonly IImageFileService _imageFileService;

    public ImageController(IImageFileService imageFileService)
    {
        _imageFileService = imageFileService;
    }

    [HttpPost("avatar/{userId:int}")]
    public async Task<ActionResult<ImageUploadResponseModel>> SetUserAvatarImage(int userId,
        [FromForm] IFormFile imageFile)
    {
        var imagePublicName = "user_avatar_" + userId;
        var result = await _imageFileService.UploadImageAsync(imageFile, imagePublicName);

        return new WebApiResult(new ImageUploadResponseModel { Url = result });
    }

    [HttpGet("avatar/{userId}")]
    public async Task<ActionResult<string>> GetUserAvatarImageLink(string userId)
    {
        var imagePublicId = "user_avatar_" + userId;
        var link = _imageFileService.GetImageLink(imagePublicId);

        return new WebApiResult(link);
    }
}