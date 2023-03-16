using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Application;
using TeachBoard.FileService.Application.Exceptions;
using TeachBoard.FileService.Application.Interfaces;
using TeachBoard.FileService.Domain.Entities;

namespace TeachBoard.FileService.Api.Controllers;

[ApiController]
[Route("file")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly ICloudFileDatabaseService _cloudFileDatabaseService;

    public FileController(IFileService fileService, ICloudFileDatabaseService cloudFileDatabaseService)
    {
        _fileService = fileService;
        _cloudFileDatabaseService = cloudFileDatabaseService;
    }

    [HttpPost("homework-solution/{studentId:int}/{homeworkId:int}")]
    public async Task<ActionResult<CloudHomeworkSolutionFileInfo>> UploadHomeworkSolutionFile(int studentId,
        int homeworkId,
        [FromForm] IFormFile file)
    {
        // generate new fileName for cloud from GUID
        var fileExtension = file.FileName.Split('.').LastOrDefault();
        var cloudFileName = Guid.NewGuid().ToString();
        if (fileExtension is not null) cloudFileName += "." + fileExtension;

        // try upload file to hosting and get TRUE if success
        var isUploadSucceed = await _fileService.UploadFileAsync(file, cloudFileName);

        var solution = isUploadSucceed
            ? await _cloudFileDatabaseService.CreateHomeworkSolution(studentId, homeworkId, file.FileName,
                cloudFileName)
            : null;

        return new WebApiResult(solution);
    }

    [HttpGet("homework-solution/{studentId:int}/{homeworkId:int}")]
    public async Task<FileContentResult> GetHomeworkSolutionFile(int studentId, int homeworkId)
    {
        var solution = await _cloudFileDatabaseService.GetHomeworkSolution(studentId, homeworkId);

        if (solution is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.FileNotFound,
                PublicErrorMessage = "Homework solution file not found"
            };

        var solutionFileBytes = await _fileService.DownloadFileAsync(solution.CloudFileName);

        return File(solutionFileBytes, "application/octet-stream", solution.OriginFileName);
    }
}