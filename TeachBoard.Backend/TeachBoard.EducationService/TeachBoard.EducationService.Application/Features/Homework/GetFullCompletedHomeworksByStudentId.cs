using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class GetFullCompletedHomeworksByStudentIdQuery : IRequest<FullCompletedHomeworksListModel>
{
    public int StudentId { get; set; }
}

public class GetFullCompletedHomeworksByStudentIdQueryHandler : IRequestHandler<
    GetFullCompletedHomeworksByStudentIdQuery, FullCompletedHomeworksListModel>
{
    private readonly IApplicationDbContext _context;

    public GetFullCompletedHomeworksByStudentIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FullCompletedHomeworksListModel> Handle(GetFullCompletedHomeworksByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        var fullCompletedHomeworks = await _context.CompletedHomeworks
            .Include(ch => ch.Homework)
            .ThenInclude(homework => homework.Subject)
            .Where(ch => ch.StudentId == request.StudentId)
            .Select(completedHomework => new FullCompletedHomework
            {
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
        
        return new FullCompletedHomeworksListModel { CompletedHomeworks = fullCompletedHomeworks };
    }
}

public class FullCompletedHomework
{
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

public class FullCompletedHomeworksListModel
{
    public IList<FullCompletedHomework> CompletedHomeworks { get; set; }
}