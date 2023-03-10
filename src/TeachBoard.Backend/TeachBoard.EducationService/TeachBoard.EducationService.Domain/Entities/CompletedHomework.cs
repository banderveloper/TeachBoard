using System.Text.Json.Serialization;

namespace TeachBoard.EducationService.Domain.Entities;

public class CompletedHomework : BaseEntity
{
    public int HomeworkId { get; set; }
    public int StudentId { get; set; }
    public int CheckingTeacherId { get; set; }
    public int? Grade { get; set; }
    public string? CheckingTeacherComment { get; set; }
    public string? StudentComment { get; set; }
    public string? FilePath { get; set; }

    //
    [JsonIgnore] public Homework Homework { get; set; }
}