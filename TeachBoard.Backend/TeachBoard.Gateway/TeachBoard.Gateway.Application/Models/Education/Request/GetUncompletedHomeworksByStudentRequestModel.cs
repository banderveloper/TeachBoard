namespace TeachBoard.Gateway.Application.Models.Education.Request;

public class GetUncompletedHomeworksByStudentRequestModel
{
    public int StudentId { get; set; }
    public int? GroupId { get; set; }
}