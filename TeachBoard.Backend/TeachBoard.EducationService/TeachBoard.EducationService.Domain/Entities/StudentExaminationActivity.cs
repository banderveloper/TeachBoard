using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.Domain.Entities;

public class StudentExaminationActivity : BaseEntity
{
    public int StudentId { get; set; }
    public int ExaminationId { get; set; }
    public int? Grade { get; set; }
    public StudentExaminationStatus Status { get; set; }
}