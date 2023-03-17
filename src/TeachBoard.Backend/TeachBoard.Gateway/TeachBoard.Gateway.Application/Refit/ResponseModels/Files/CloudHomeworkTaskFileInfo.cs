namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Files;

public class CloudHomeworkTaskFileInfo
{
    public int Id { get; set; }
    public int HomeworkId { get; set; }
    public string OriginFileName { get; set; } = string.Empty;
    public string CloudFileName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}