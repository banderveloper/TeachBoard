namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

public class StudentExaminationActivity
{
    public int StudentId { get; set; }
    public int ExaminationId { get; set; }
    public int? Grade { get; set; }
    public StudentExaminationStatus Status { get; set; }
}