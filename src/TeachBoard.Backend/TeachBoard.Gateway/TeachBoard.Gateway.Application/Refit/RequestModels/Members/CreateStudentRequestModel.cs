namespace TeachBoard.Gateway.Application.Refit.RequestModels.Members;

public class CreateStudentRequestModel
{
    public int UserId { get; set; }
    public int? GroupId { get; set; }
}