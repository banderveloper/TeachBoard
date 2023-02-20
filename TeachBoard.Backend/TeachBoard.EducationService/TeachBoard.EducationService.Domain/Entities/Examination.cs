using System.Text.Json.Serialization;

namespace TeachBoard.EducationService.Domain.Entities;

public class Examination : BaseEntity
{
    public int SubjectId { get; set; }
    public int GroupId { get; set; }
    public DateTime StarsAt { get; set; }
    public DateTime EndsAt { get; set; }
    
    //
    [JsonIgnore] public Subject Subject { get; set; }
}