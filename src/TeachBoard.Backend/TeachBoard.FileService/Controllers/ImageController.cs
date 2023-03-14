using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Interfaces;

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

    [HttpPost("avatar/{userId:int}")]
    public async Task<ActionResult<ImageUploadResult>> SetUserAvatarImage(int userId, [FromForm] IFormFile imageFile)
    {
        var imagePublicId = "user_avatar_" + userId;
        var result = await _imageFileService.UploadImageAsync(imageFile, imagePublicId);

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