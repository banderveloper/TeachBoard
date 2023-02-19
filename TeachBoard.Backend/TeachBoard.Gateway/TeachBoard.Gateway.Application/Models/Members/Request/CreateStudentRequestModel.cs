namespace TeachBoard.Gateway.Application.Models.Members.Request;

public class CreateStudentRequestModel
{
    public int UserId { get; set; }
    public int? GroupId { get; set; }
}