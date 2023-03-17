namespace TeachBoard.Gateway.WebApi.Models;

public class CreateHomeworkRequestModel
{
    public int GroupId { get; set; }
    public int SubjectId { get; set; }
    public IFormFile File { get; set; }
}