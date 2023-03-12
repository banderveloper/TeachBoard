namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class StudentLessonActivity
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
    public int? Grade { get; set; }
}