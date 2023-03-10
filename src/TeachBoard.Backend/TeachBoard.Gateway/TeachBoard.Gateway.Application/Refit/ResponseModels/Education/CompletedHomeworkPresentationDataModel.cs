namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class CompletedHomeworkPresentationDataModel
{
    public int StudentId { get; set; }
    public int CompletedHomeworkId { get; set; }
    public int HomeworkId { get; set; }
    public int CheckingTeacherId { get; set; }
    public int? Grade { get; set; }
    public string? CheckingTeacherComment { get; set; }
    public string? SolutionFilePath { get; set; }
    public string? TaskFilePath { get; set; }
    public string SubjectName { get; set; }
    public DateTime CreatedAt { get; set; }
}