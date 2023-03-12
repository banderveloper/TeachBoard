using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

namespace TeachBoard.Gateway.Application.Refit.RequestModels.Education;

public class SetStudentLessonActivityRequestModel
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    
    public AttendanceStatus AttendanceStatus { get; set; }
    public int? Grade { get; set; }
}