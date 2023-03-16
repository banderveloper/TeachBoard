namespace TeachBoard.FileService.Api.Models;

public class HomeworkFileResponseModel
{
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}