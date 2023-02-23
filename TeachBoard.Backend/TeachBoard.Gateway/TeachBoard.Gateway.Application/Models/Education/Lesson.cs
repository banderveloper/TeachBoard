namespace TeachBoard.Gateway.Application.Models.Education;

public class Lesson
{
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public int GroupId { get; set; }
    public string? Topic { get; set; }
    public string? Classroom { get; set; }

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}