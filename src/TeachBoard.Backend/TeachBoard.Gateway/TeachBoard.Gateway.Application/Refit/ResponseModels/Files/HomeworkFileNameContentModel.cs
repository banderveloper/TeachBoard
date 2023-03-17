namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Files;

public class HomeworkFileNameContentModel
{
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}