namespace TeachBoard.Gateway.Application.Refit.RequestModels.Education;

public class CreateHomeworkInternalRequestModel
{
    public int GroupId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public string FilePath { get; set; } = string.Empty;
}