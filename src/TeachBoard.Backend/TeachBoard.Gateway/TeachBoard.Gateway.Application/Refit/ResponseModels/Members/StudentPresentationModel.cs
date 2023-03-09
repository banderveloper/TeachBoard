namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Members;

public class StudentPresentationModel
{
    public int UserId { get; set; }
    public int StudentId { get; set; }
    public Group? Group { get; set; }
}