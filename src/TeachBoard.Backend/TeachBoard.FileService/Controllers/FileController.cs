using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Interfaces;
using TeachBoard.FileService.Models;

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

    [HttpPost("homework-solution")]
    public async Task<IActionResult> UploadHomeworkSolutionFile([FromForm] UploadHomeworkSolutionRequestModel model)
    {
        // get upload file extension without dot
        var fileExtension = model.HomeworkFile.FileName.Split('.').LastOrDefault();

        // set filename (example: hws_5_10.txt). hws = homework solution 
        var fileName = $"hws_{model.StudentId}_{model.HomeworkId}";
        if (fileExtension is not null) fileName += $".{fileExtension}";

        var isSucceed = await _fileService.UploadFileAsync(model.HomeworkFile, fileName);

        return Ok(new { isSucceed });
    }
    
    [HttpGet("homework-solution")]
    public async Task<IActionResult> GetHomeworkSolutionFile()
    {
        var fileName = "5_10.png";
        var bytes = await _fileService.DownloadFileAsync(fileName);

        return File(bytes, "application/octet-stream", "iloveyou.png");
    }
}