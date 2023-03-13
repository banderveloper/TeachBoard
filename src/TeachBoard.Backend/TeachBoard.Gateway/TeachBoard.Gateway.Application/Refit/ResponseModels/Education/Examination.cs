namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class Examination 
{
    public int SubjectId { get; set; }
    public int GroupId { get; set; }
    public DateTime StarsAt { get; set; }
    public DateTime EndsAt { get; set; }
}