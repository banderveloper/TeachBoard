using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<ActionResult<UploadResult>> UploadFile(IFormFile file)
    {
        return await _imageFileService.UploadAsync(file);
    }
}