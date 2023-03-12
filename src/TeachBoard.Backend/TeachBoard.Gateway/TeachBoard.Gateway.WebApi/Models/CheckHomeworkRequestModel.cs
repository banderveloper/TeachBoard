namespace TeachBoard.Gateway.WebApi.Models;

public class CheckHomeworkRequestModel
{
    public int CompletedHomeworkId { get; set; }
    public int Grade { get; set; }
    public string? Comment { get; set; }
}