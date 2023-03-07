namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class LessonPresentationDataModel
{
    public string SubjectName { get; set; }
    public int TeacherId { get; set; }
    public string? Topic { get; set; }
    public string? Classroom { get; set; }

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}