using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Interfaces;
using TeachBoard.FileService.Services;

namespace TeachBoard.FileService.Controllers;

[ApiController]
[Route("file")]
public class FileController : ControllerBase
{
    private readonly IImageFileService _imageFileService;

    public FileController(IImageFileService imageFileService)
    {
        _imageFileService = imageFileService;
    }

    [HttpPost("image")]
    public async Task<ActionResult<UploadResult>> UploadImage(IFormFile imageFile)
    {
        return await _imageFileService.UploadAsync(imageFile);
    }

    [HttpGet("image/{publicId}")]
    public async Task<ActionResult<string>> GetImageLink(string publicId)
    {
        return await _imageFileService.GetImageLinkAsync(publicId);
    }
}