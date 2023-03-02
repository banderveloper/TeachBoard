using System.Text.Json.Serialization;

namespace TeachBoard.Gateway.Application.Models.Education.Request;

public class CompleteHomeworkRequestModel
{
    public int HomeworkId { get; set; }
    public int? StudentGroupId { get; set; }
    public int? StudentId { get; set; }
    public string? FilePath { get; set; }
    public string? StudentComment { get; set; }
}