namespace TeachBoard.Gateway.Application.Refit.RequestModels.Education;

public class CheckHomeworkInternalRequestModel
{
    public int TeacherId { get; set; }
    public int CompletedHomeworkId { get; set; }
    public int Grade { get; set; }
    public string? Comment { get; set; }
}