using System.Text.Json.Serialization;
using TeachBoard.MembersService.Domain.Enums;

namespace TeachBoard.MembersService.Domain.Entities;

public class Feedback : BaseEntity
{
    public int TeacherId { get; set; }
    public int StudentId { get; set; }
    public FeedbackDirection Direction { get; set; }
    public string? Text { get; set; }
    public int Rating { get; set; }

    [JsonIgnore] public Student? Student { get; set; }
    [JsonIgnore] public Teacher? Teacher { get; set; }
}