namespace TeachBoard.FileService.Domain.Entities;

public class CloudHomeworkSolutionFileInfo : BaseEntity
{
    public int StudentId { get; set; }
    public int HomeworkId { get; set; }
    public string OriginFileName { get; set; } = string.Empty;
    public string CloudFileName { get; set; } = string.Empty;
}