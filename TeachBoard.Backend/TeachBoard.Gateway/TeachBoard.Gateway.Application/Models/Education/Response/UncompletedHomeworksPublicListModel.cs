namespace TeachBoard.Gateway.Application.Models.Education.Response;

public class UncompletedHomeworksPublicListModel
{
    public IList<UncompletedHomeworkPublicModel> UncompletedHomeworks { get; set; }
}

public class UncompletedHomeworkPublicModel
{
    public int HomeworkId { get; set; }
    public string SubjectName { get; set; }
    public int TeacherId { get; set; }
    public string FilePath { get; set; }
    public DateTime CreatedAt { get; set; }
}

