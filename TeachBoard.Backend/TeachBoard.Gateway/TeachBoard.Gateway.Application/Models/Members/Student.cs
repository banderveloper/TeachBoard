namespace TeachBoard.Gateway.Application.Models.Members;

public class Student
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? GroupId { get; set; }
    public DateTime CreatedAt { get; set; }
}