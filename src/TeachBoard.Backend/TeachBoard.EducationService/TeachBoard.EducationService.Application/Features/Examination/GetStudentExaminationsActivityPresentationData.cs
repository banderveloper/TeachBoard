using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Converters;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.Application.Features.Examination;

// Get public data of student examination activities (examId, subjectName, grade, status)

public class
    GetStudentExaminationsActivityPresentationDataModelQuery : IRequest<IList<StudentExaminationActivityPresentationDataModel>>
{
    public int StudentId { get; set; }
}

public class GetStudentExaminationsActivityPresentationDataModelQueryHandler : IRequestHandler<
    GetStudentExaminationsActivityPresentationDataModelQuery,
    IList<StudentExaminationActivityPresentationDataModel>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentExaminationsActivityPresentationDataModelQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<StudentExaminationActivityPresentationDataModel>> Handle(
        GetStudentExaminationsActivityPresentationDataModelQuery request,
        CancellationToken cancellationToken)
    {
        // Get given student examination activities, join examinations and subjects names 

        var studentExaminationsActivities = await _context.StudentExaminationActivities
            .Where(sta => sta.StudentId == request.StudentId)
            .Include(sea => sea.Examination)
            .ThenInclude(e => e.Subject)
            .Select(sea => new StudentExaminationActivityPresentationDataModel
            {
                StudentId = sea.StudentId,
                ExaminationId = sea.ExaminationId,
                Grade = sea.Grade,
                Status = sea.Status,
                SubjectName = sea.Examination.Subject.Name
            })
            .ToListAsync(cancellationToken);

        return studentExaminationsActivities;
    }
}

public class StudentExaminationActivityPresentationDataModel
{
    public int StudentId { get; set; }
    public int ExaminationId { get; set; }
    public string SubjectName { get; set; }
    public int? Grade { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter<StudentExaminationStatus>))]
    public StudentExaminationStatus Status { get; set; }
}