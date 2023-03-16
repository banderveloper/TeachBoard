using Microsoft.EntityFrameworkCore;
using TeachBoard.FileService.Application.Interfaces;
using TeachBoard.FileService.Domain.Entities;

namespace TeachBoard.FileService.Application.Services;

public class CloudFileDatabaseService : ICloudFileDatabaseService
{
    private readonly IApplicationDbContext _context;

    public CloudFileDatabaseService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CloudHomeworkSolutionFileInfo> CreateHomeworkSolution(int studentId, int homeworkId, string originFileName, string cloudFileName)
    {
        var newSolution = new CloudHomeworkSolutionFileInfo
        {
            StudentId = studentId,
            HomeworkId = homeworkId,
            OriginFileName = originFileName,
            CloudFileName = cloudFileName
        };

        _context.HomeworkSolutions.Add(newSolution);
        await _context.SaveChangesAsync(CancellationToken.None);
        return newSolution;
    }

    public async Task<CloudHomeworkSolutionFileInfo?> GetHomeworkSolution(int studentId, int homeworkId)
    {
        return await _context.HomeworkSolutions.FirstOrDefaultAsync(sln =>
            sln.HomeworkId == homeworkId && sln.StudentId == studentId);
    }
   
}