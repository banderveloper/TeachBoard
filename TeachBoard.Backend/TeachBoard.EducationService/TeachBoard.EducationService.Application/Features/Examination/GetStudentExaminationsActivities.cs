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
        // Get all student examination activities by student id 
        var studentExaminationsActivities = await _context.StudentExaminationActivities
            .Where(sta => sta.StudentId == request.StudentId)
            .ToListAsync(cancellationToken);

        // if student has no activities - ex
        if (studentExaminationsActivities.Count == 0)
            throw new NotFoundException
            {
                Error = "student_examination_activities_not_found",
                ErrorDescription = $"Examination activities of student with id '{request.StudentId}' not found",
                ReasonField = "studentId"
            };

        // Extract list of examination ids from activities 
        var activitiesExaminationIds = studentExaminationsActivities.Select(sea => sea.ExaminationId);
        
        // Get examinations list by ids of examinations from activities
        // it made for get examination subject name
        var examinations = await _context.Examinations
            .Include(e => e.Subject)
            .Where(e => activitiesExaminationIds.Contains(e.Id))
            .ToListAsync(cancellationToken);

        // Result model with public data of examinations
        var result = new StudentExaminationsPublicDataListModel
            { Examinations = new List<StudentExaminationPublicDataDto>() };

        // Iterate over activities and create public examination data
        // todo refactor getting subjectName
        foreach (var activity in studentExaminationsActivities)
        {
            result.Examinations.Add(new StudentExaminationPublicDataDto()
            {
                ExaminationId = activity.ExaminationId,
                Grade = activity.Grade,
                Status = activity.Status,
                SubjectName = examinations.FirstOrDefault(e => e.Id == activity.ExaminationId).Subject.Name
            });
        }

        return result;
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