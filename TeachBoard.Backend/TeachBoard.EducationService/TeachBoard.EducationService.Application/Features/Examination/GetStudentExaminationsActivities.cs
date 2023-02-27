using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.Application.Features.Examination;

// Get public data of student examination activites (examId, subjectName, grade, status)

public class GetStudentExaminationsActivitiesQuery : IRequest<StudentExaminationsPublicDataListModel>
{
    public int StudentId { get; set; }
}

public class GetStudentExaminationActivitiesQueryHandler : IRequestHandler<GetStudentExaminationsActivitiesQuery,
    StudentExaminationsPublicDataListModel>
{
    private readonly IApplicationDbContext _context;

    public GetStudentExaminationActivitiesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentExaminationsPublicDataListModel> Handle(GetStudentExaminationsActivitiesQuery request,
        CancellationToken cancellationToken)
    {
        // Get given student examination activities, join examinations and subjects names 
        var studentExaminationsActivities = await _context.StudentExaminationActivities
            .Where(sta => sta.StudentId == request.StudentId)
            .Include(sea => sea.Examination)
            .ThenInclude(e => e.Subject)
            .Select(sea => new StudentExaminationPublicDataDto
            {
                ExaminationId = sea.ExaminationId,
                Grade = sea.Grade,
                Status = sea.Status,
                SubjectName = sea.Examination.Subject.Name
            })
            .ToListAsync(cancellationToken);

        if (studentExaminationsActivities.Count == 0)
            throw new NotFoundException
            {
                Error = "student_examination_activities_not_found",
                ErrorDescription = $"Examination activities of student with id '{request.StudentId}' not found",
                ReasonField = "studentId"
            };

        return new StudentExaminationsPublicDataListModel
        {
            Examinations = studentExaminationsActivities
        };
    }
}

public class StudentExaminationPublicDataDto
{
    public int ExaminationId { get; set; }
    public string SubjectName { get; set; }
    public int? Grade { get; set; }
    public StudentExaminationStatus Status { get; set; }
}

public class StudentExaminationsPublicDataListModel
{
    public IList<StudentExaminationPublicDataDto> Examinations { get; set; }
}