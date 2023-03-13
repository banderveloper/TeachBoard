using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Interfaces;
using TeachBoard.FileService.Models;

namespace TeachBoard.FileService.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    private readonly IImageFileService _imageFileService;

    public ImageController(IImageFileService imageFileService)
    {
        _imageFileService = imageFileService;
    }

    [HttpPost("avatar")]
    public async Task<ActionResult<ImageUploadResult>> SetUserAvatarImage([FromForm] SetUserAvatarRequestModel model)
    {
        var imagePublicId = "user_avatar_" + model.UserId;
        var result = await _imageFileService.UploadImageAsync(model.ImageFile, imagePublicId);

        return new WebApiResult(result);
    }

    [HttpGet("avatar/{userId}")]
    public async Task<ActionResult<string>> GetUserAvatarImageLink(string userId)
    {
        var imagePublicId = "user_avatar_" + userId;
        var link = _imageFileService.GetImageLink(imagePublicId);

        return new WebApiResult(link);
    }
}