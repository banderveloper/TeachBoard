namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class StudentLessonActivityPresentationDataModel
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public string LessonTopic { get; set; }
    public string SubjectName { get; set; }
    
    public AttendanceStatus AttendanceStatus { get; set; }

    public int? Grade { get; set; }
    public DateTime ActivityCreatedAt { get; set; }
}