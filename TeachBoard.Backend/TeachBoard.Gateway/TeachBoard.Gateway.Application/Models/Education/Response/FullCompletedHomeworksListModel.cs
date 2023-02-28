namespace TeachBoard.Gateway.Application.Models.Education.Response;

public class FullCompletedHomeworksListModel
{
    public IList<FullCompletedHomework> CompletedHomeworks { get; set; }
}

public class FullCompletedHomework
{
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