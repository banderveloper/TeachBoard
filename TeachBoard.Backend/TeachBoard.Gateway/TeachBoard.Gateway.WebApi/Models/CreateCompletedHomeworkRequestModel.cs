namespace TeachBoard.Gateway.WebApi.Models;

public class CreateCompletedHomeworkRequestModel
{
    public int HomeworkId { get; set; }
    public string? FilePath { get; set; }
    public string? StudentComment { get; set; }
}