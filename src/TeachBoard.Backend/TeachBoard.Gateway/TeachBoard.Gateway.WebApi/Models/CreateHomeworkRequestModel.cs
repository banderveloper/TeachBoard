namespace TeachBoard.Gateway.WebApi.Models;

public class CreateHomeworkRequestModel
{
    public int GroupId { get; set; }
    public int SubjectId { get; set; }
    public string FilePath { get; set; }
}