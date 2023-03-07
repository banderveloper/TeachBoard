namespace TeachBoard.Gateway.Application.Refit.RequestModels.Education;

public class CompleteHomeworkInternalRequestModel 
{
    public int HomeworkId { get; set; }
    public int StudentId { get; set; }
    public int? StudentGroupId { get; set; }
    public string? FilePath { get; set; }
    public string? StudentComment { get; set; }
}