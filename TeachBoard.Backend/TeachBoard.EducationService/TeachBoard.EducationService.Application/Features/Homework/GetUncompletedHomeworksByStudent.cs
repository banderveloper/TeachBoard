using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class GetUncompletedHomeworksByStudentQuery : IRequest<UncompletedHomeworksPublicListModel>
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }
}

public class GetUncompletedHomeworksByStudentQueryHandler : IRequestHandler<GetUncompletedHomeworksByStudentQuery,
    UncompletedHomeworksPublicListModel>
{
    private readonly IApplicationDbContext _context;

    public GetUncompletedHomeworksByStudentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UncompletedHomeworksPublicListModel> Handle(GetUncompletedHomeworksByStudentQuery request,
        CancellationToken cancellationToken)
    {
        // completed homeworks by student
        var studentCompletedHomeworkIds = await _context.CompletedHomeworks
            .Where(ch => ch.StudentId == request.StudentId)
            .Select(ch => ch.HomeworkId)
            .ToListAsync(cancellationToken);

        // get all the homework for the group, excluding those whose id is in the list of completed
        var uncompletedHomeworks =
            await _context.Homeworks
                .Include(h => h.Subject)
                .Where(h => h.GroupId == request.GroupId && !studentCompletedHomeworkIds.Contains(h.Id))
                .Select(h => new UncompletedHomeworkPublicModel
                {
                    HomeworkId = h.Id,
                    TeacherId = h.TeacherId,
                    SubjectName = h.Subject.Name,
                    FilePath = h.FilePath,
                    CreatedAt = h.CreatedAt
                })
                .ToListAsync(cancellationToken);

        
        if (uncompletedHomeworks.Count == 0)
            throw new NotFoundException
            {
                Error = "uncompleted_homeworks_not_found",
                ErrorDescription = $"Student with id '{request.StudentId}' does not have uncompleted homeworks"
            };

        return new UncompletedHomeworksPublicListModel { UncompletedHomeworks = uncompletedHomeworks };
    }
}

public class UncompletedHomeworkPublicModel
{
    public int HomeworkId { get; set; }
    public string SubjectName { get; set; }
    public int TeacherId { get; set; }
    public string FilePath { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UncompletedHomeworksPublicListModel
{
    public IList<UncompletedHomeworkPublicModel> UncompletedHomeworks { get; set; }
}