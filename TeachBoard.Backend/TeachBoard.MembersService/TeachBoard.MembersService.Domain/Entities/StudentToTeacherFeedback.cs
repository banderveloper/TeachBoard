using System.Text.Json.Serialization;

namespace TeachBoard.MembersService.Domain.Entities;

public class StudentToTeacherFeedback : BaseEntity
{
    public int TeacherId { get; set; }
    public int StudentId { get; set; }
    
    public string? Text { get; set; }
    public int Rating { get; set; }
    
    [JsonIgnore] public Student? Student { get; set; }
    [JsonIgnore] public Teacher? Teacher { get; set; }
}