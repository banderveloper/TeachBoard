using TeachBoard.FileService.Domain.Entities;

namespace TeachBoard.FileService.Application.Interfaces;

public interface ICloudFileDatabaseService
{
    Task<CloudHomeworkSolutionFileInfo> CreateHomeworkSolution(int studentId, int homeworkId, string originFileName,
        string cloudFileName);

    Task<CloudHomeworkSolutionFileInfo?> GetHomeworkSolution(int studentId, int homeworkId);
}