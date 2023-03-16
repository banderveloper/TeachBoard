namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Files;

public class HomeworkSolutionFile
{
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}