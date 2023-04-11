namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class Lesson
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public int GroupId { get; set; }
    public string? Topic { get; set; }
    public string? Classroom { get; set; }

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}