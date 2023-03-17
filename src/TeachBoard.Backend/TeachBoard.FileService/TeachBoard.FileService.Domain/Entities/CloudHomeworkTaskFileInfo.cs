namespace TeachBoard.FileService.Domain.Entities;

public class CloudHomeworkTaskFileInfo : BaseEntity
{
    public int HomeworkId { get; set; }
    public string OriginFileName { get; set; } = string.Empty;
    public string CloudFileName { get; set; } = string.Empty;
}