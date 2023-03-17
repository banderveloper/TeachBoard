using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Refit;
using TeachBoard.Gateway.Application.Refit.RequestModels.Files;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Files;

namespace TeachBoard.Gateway.Application.Refit.Clients;

public interface IFilesClient
{
    [Multipart]
    [Post("/image/avatar/{userId}")]
    Task<ServiceTypedResponse<ImageUploadResult>> SetUserAvatar(int userId, [AliasAs("imageFile")] StreamPart stream);

    [Get("/file/homework-solution/{studentId}/{homeworkId}")]
    Task<ServiceTypedResponse<HomeworkSolutionFile?>> GetHomeworkSolutionFile(int studentId, int homeworkId);

    [Multipart]
    [Post("/file/homework-solution/{studentId}/{homeworkId}")]
    Task<ServiceTypedResponse<CloudHomeworkSolutionFileInfo>> UploadHomeworkSolutionFile(int studentId, int homeworkId,
        [AliasAs("file")] StreamPart file);
}