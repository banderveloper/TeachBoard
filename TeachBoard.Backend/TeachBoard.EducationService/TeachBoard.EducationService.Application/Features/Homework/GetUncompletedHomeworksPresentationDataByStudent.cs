using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class GetUncompletedHomeworksPresentationDataByStudentQuery : IRequest<IList<UncompletedHomeworkPresentationDataModel>>
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }
}

public class GetUncompletedHomeworksPresentationDataByStudentQueryHandler : IRequestHandler<GetUncompletedHomeworksPresentationDataByStudentQuery,
    IList<UncompletedHomeworkPresentationDataModel>>
{
    private readonly IApplicationDbContext _context;

    public GetUncompletedHomeworksPresentationDataByStudentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<UncompletedHomeworkPresentationDataModel>> Handle(
        GetUncompletedHomeworksPresentationDataByStudentQuery request,
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
                .Select(h => new UncompletedHomeworkPresentationDataModel
                {
                    HomeworkId = h.Id,
                    TeacherId = h.TeacherId,
                    SubjectName = h.Subject.Name,
                    FilePath = h.FilePath,
                    CreatedAt = h.CreatedAt
                })
                .ToListAsync(cancellationToken);

        return uncompletedHomeworks;
    }
}

public class UncompletedHomeworkPresentationDataModel
{
    public int HomeworkId { get; set; }
    public string SubjectName { get; set; }
    public int TeacherId { get; set; }
    public string FilePath { get; set; }
    public DateTime CreatedAt { get; set; }
}