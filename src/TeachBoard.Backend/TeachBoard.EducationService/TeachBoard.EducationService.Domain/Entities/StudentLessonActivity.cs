using System.Text.Json.Serialization;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.Domain.Entities;

public class StudentLessonActivity : BaseEntity
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
    public int? Grade { get; set; }
    
    //
    [JsonIgnore] public Lesson Lesson { get; set; }
}