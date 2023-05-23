namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int LessonsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}