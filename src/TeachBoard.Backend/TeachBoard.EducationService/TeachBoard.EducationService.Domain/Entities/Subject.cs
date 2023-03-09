namespace TeachBoard.EducationService.Domain.Entities;

public class Subject : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int LessonsCount { get; set; }
}