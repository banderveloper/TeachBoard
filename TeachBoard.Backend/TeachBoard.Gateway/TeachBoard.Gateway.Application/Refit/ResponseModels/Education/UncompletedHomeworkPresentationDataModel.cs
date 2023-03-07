namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class UncompletedHomeworkPresentationDataModel
{
    public int HomeworkId { get; set; }
    public string SubjectName { get; set; }
    public int TeacherId { get; set; }
    public string FilePath { get; set; }
    public DateTime CreatedAt { get; set; }
}