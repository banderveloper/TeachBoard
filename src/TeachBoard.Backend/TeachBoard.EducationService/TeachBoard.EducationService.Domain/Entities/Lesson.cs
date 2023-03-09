using System.Text.Json.Serialization;

namespace TeachBoard.EducationService.Domain.Entities;

public class Lesson : BaseEntity
{
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public int GroupId { get; set; }
    public string? Topic { get; set; }
    public string? Classroom { get; set; }

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }

    //
    [JsonIgnore] public Subject Subject { get; set; }
}