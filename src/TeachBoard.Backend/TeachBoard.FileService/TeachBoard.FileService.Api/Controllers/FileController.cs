using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Application.Interfaces;

namespace TeachBoard.FileService.Api.Controllers;

[ApiController]
[Route("file")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("homework-solution/{studentId:int}/{homeworkId:int}")]
    public async Task<IActionResult> UploadHomeworkSolutionFile(int studentId, int homeworkId, [FromForm] IFormFile file)
    {
        // get upload file extension without dot
        var fileExtension = file.FileName.Split('.').LastOrDefault();

        // set filename (example: hws_5_10.txt). hws = homework solution 
        var fileName = $"hws_{studentId}_{homeworkId}";
        if (fileExtension is not null) fileName += $".{fileExtension}";

        var isSucceed = await _fileService.UploadFileAsync(file, fileName);

        return Ok(new { isSucceed });
    }
    
    [HttpGet("homework-solution/{studentId:int}/{homeworkId:int}")]
    public async Task<IActionResult> GetHomeworkSolutionFile(int studentId, int homeworkId)
    {
        var fileName = "5_10.png";
        var bytes = await _fileService.DownloadFileAsync(fileName);

        return File(bytes, "application/octet-stream", "iloveyou.png");
    }
}