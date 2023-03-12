using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

namespace TeachBoard.Gateway.WebApi.Models;

public class FullLessonInfoResponseModel
{
   public Lesson Lesson { get; set; }
   public IEnumerable<StudentPresentationWithActivityModel> Students { get; set; }
}

public class StudentPresentationWithActivityModel
{
    public int UserId { get; set; }
    public int StudentId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }

    public string? AvatarImagePath { get; set; }
    
    public int? Grade { get; set; }
    public AttendanceStatus? AttendanceStatus { get; set; }
}