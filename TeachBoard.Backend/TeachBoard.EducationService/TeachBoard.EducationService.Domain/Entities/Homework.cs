using System.Text.Json.Serialization;

namespace TeachBoard.EducationService.Domain.Entities;

public class Homework : BaseEntity
{
    public int GroupId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public string FilePath { get; set; }

    //
    [JsonIgnore] public Subject Subject { get; set; }
}