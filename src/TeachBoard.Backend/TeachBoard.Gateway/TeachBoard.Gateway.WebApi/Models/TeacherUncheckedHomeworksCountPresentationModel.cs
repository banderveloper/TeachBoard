namespace TeachBoard.Gateway.WebApi.Models;

public class TeacherUncheckedHomeworksCountPresentationModel
{
    public int UserId { get; set; }
    public int TeacherId { get; set; }
    public string TeacherFullName { get; set; }
    public int HomeworksCount { get; set; }
}