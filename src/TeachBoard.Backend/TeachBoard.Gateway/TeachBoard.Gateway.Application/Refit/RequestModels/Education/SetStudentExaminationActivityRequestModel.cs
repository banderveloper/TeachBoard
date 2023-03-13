using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

namespace TeachBoard.Gateway.Application.Refit.RequestModels.Education;

public class SetStudentExaminationActivityRequestModel
{
    public int? Grade { get; set; }

    public int StudentId { get; set; }
    public int ExaminationId { get; set; }

    public StudentExaminationStatus Status { get; set; }
}