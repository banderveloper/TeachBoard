using System.Text.Json.Serialization;

namespace TeachBoard.EducationService.Domain.Entities;

public class CompletedHomework : BaseEntity
{
    public int HomeworkId { get; set; }
    public int StudentId { get; set; }
    public int? Grade { get; set; }
    public string? Comment { get; set; }
    public string? FilePath { get; set; }
    
    //
    [JsonIgnore] public Homework Homework { get; set; }
}