namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class TeacherUncheckedHomeworksCountModel
{
    public int TeacherId { get; set; }
    public int Count { get; set; } // amount of unchecked homeworks
}