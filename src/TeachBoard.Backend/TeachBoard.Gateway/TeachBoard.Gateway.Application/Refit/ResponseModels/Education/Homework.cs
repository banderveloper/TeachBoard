using System.Text.Json.Serialization;

namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class Homework
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public string FilePath { get; set; }
    public DateTime CreatedAt { get; set; }
}