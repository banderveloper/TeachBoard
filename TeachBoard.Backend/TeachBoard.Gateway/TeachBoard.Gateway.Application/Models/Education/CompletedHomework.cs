namespace TeachBoard.Gateway.Application.Models.Education;

public class CompletedHomework
{
    public int HomeworkId { get; set; }
    public int StudentId { get; set; }
    public int CheckingTeacherId { get; set; }
    public int? Grade { get; set; }
    public string? CheckingTeacherComment { get; set; }
    public string? StudentComment { get; set; }
    public string? FilePath { get; set; }
}