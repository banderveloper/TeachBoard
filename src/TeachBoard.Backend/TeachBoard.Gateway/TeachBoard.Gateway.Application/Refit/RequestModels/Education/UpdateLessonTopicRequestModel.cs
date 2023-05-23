namespace TeachBoard.Gateway.Application.Refit.RequestModels.Education;

public class UpdateLessonTopicRequestModel
{
    public int LessonId { get; set; }
    public string Topic { get; set; }
}