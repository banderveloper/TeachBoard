using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class GetCompletedHomeworksPresentationDataByStudentIdQuery : IRequest<IList<CompletedHomeworkPresentationDataModel>>
{
    public int StudentId { get; set; }
}

public class GetCompletedHomeworksPresentationDataByStudentIdQueryHandler : IRequestHandler<
    GetCompletedHomeworksPresentationDataByStudentIdQuery, IList<CompletedHomeworkPresentationDataModel>>
{
    private readonly IApplicationDbContext _context;

    public GetCompletedHomeworksPresentationDataByStudentIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<CompletedHomeworkPresentationDataModel>> Handle(GetCompletedHomeworksPresentationDataByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        // Get completed homeworks with full data joining subjects and homeworks
        var completedHomeworks = await _context.CompletedHomeworks
            .Include(ch => ch.Homework)
            .ThenInclude(homework => homework.Subject)
            .Where(ch => ch.StudentId == request.StudentId)
            .Select(completedHomework => new CompletedHomeworkPresentationDataModel
            {
                StudentId = completedHomework.StudentId,
                HomeworkId = completedHomework.HomeworkId,
                Grade = completedHomework.Grade,
                CheckingTeacherComment = completedHomework.CheckingTeacherComment,
                CheckingTeacherId = completedHomework.CheckingTeacherId,
                CompletedHomeworkId = completedHomework.Id,
                SolutionFilePath = completedHomework.FilePath,
                TaskFilePath = completedHomework.Homework.FilePath,
                SubjectName = completedHomework.Homework.Subject.Name,
                CreatedAt = completedHomework.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return completedHomeworks;
    }
}

public class CompletedHomeworkPresentationDataModel
{
    public int StudentId { get; set; }
    public int CompletedHomeworkId { get; set; }
    public int HomeworkId { get; set; }
    public int CheckingTeacherId { get; set; }
    public int? Grade { get; set; }
    public string? CheckingTeacherComment { get; set; }
    public string? SolutionFilePath { get; set; }
    public string? TaskFilePath { get; set; }
    public string SubjectName { get; set; }
    public DateTime CreatedAt { get; set; }
}