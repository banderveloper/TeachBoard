namespace TeachBoard.Gateway.Application.Refit.RequestModels.Education;

public class CreateExaminationRequestModel
{
    public int SubjectId { get; set; }
    public int CheckingTeacherId { get; set; }
    public int GroupId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}