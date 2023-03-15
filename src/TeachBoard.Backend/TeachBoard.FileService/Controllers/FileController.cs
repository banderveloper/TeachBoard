using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Interfaces;

namespace TeachBoard.FileService.Controllers;

[ApiController]
[Route("file")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        await _fileService.Test();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        await _fileService.UploadFileAsync(file, "iloveyou");
        return Ok();
    }
}