using TeachBoard.Gateway.Domain.Enums;

namespace TeachBoard.Gateway.Application.Models.Education.Response;

public class StudentExaminationsPublicDataListModel
{
    public IList<StudentExaminationPublicDataDto> Examinations { get; set; }
}

public class StudentExaminationPublicDataDto
{
    public int ExaminationId { get; set; }
    public string SubjectName { get; set; }
    public int? Grade { get; set; }
    public StudentExaminationStatus Status { get; set; }
}

