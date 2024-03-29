using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.FileService.Api.Models;
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

        if (!isUploadSucceed)
            throw new HostingErrorException
            {
                ErrorCode = ErrorCode.HostingBadResponse,
                PublicErrorMessage = "Error during uploading homework file"
            };

        // return created db solution if succeed
        var solution = await _cloudFileDatabaseService.CreateHomeworkSolution(studentId, homeworkId, file.FileName,
            cloudFileName);

        return new WebApiResult(solution);
    }

    [HttpGet("homework-solution/{studentId:int}/{homeworkId:int}")]
    public async Task<ActionResult<HomeworkFileResponseModel>> GetHomeworkSolutionFile(int studentId, int homeworkId)
    {
        // get file info from db to get origin and cloud filename
        var solution = await _cloudFileDatabaseService.GetHomeworkSolution(studentId, homeworkId);

        if (solution is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.FileInfoNotFound,
                PublicErrorMessage = "Homework solution file not found"
            };

        // download file from hosting and get bytes
        var solutionFileBytes = await _fileService.DownloadFileAsync(solution.CloudFileName);

        var response = new HomeworkFileResponseModel
        {
            FileName = solution.OriginFileName,
            FileContent = solutionFileBytes
        };

        return new WebApiResult(response);
    }

    [HttpPost("homework-task/{homeworkId:int}")]
    public async Task<ActionResult<CloudHomeworkTaskFileInfo>> UploadHomeworkTaskFile(int homeworkId,
        [FromForm] IFormFile file)
    {
        // generate new fileName for cloud from GUID
        var fileExtension = file.FileName.Split('.').LastOrDefault();
        var cloudFileName = Guid.NewGuid().ToString();
        if (fileExtension is not null) cloudFileName += "." + fileExtension;

        // try upload file to hosting and get TRUE if success
        var isUploadSucceed = await _fileService.UploadFileAsync(file, cloudFileName);

        if (!isUploadSucceed)
            throw new HostingErrorException
            {
                ErrorCode = ErrorCode.HostingBadResponse,
                PublicErrorMessage = "Error during uploading homework task file"
            };

        // return created db task if succeed
        var homeworkTaskInfo =
            await _cloudFileDatabaseService.CreateHomeworkTask(homeworkId, file.FileName, cloudFileName);

        return new WebApiResult(homeworkTaskInfo);
    }

    [HttpGet("homework-task/{homeworkId:int}")]
    public async Task<ActionResult<HomeworkFileResponseModel>> GetHomeworkTaskFile(int homeworkId)
    {
        // get file info from db to get origin and cloud filename
        var solution = await _cloudFileDatabaseService.GetHomeworkTask(homeworkId);

        if (solution is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.FileInfoNotFound,
                PublicErrorMessage = "Homework task file not found"
            };

        // download file from hosting and get bytes
        var solutionFileBytes = await _fileService.DownloadFileAsync(solution.CloudFileName);

        var response = new HomeworkFileResponseModel
        {
            FileName = solution.OriginFileName,
            FileContent = solutionFileBytes
        };

        return new WebApiResult(response);
    }
}