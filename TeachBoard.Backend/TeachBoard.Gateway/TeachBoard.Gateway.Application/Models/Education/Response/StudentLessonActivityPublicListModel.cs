using TeachBoard.Gateway.Domain.Enums;

namespace TeachBoard.Gateway.Application.Models.Education.Response;

public class StudentLessonActivityPublicListModel
{
    public IList<StudentLessonActivityPublicModel> Activities { get; set; }
}

public class StudentLessonActivityPublicModel
{
    public int LessonId { get; set; }
    public string LessonTopic { get; set; }
    public string SubjectName { get; set; }
    public StudentAttendanceStatus AttendanceStatus { get; set; }
    public int? Grade { get; set; }
    public DateTime ActivityCreatedAt { get; set; }
}

