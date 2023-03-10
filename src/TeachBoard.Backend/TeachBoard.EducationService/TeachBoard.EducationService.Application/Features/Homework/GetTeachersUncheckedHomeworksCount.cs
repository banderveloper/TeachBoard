using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class GetTeachersUncheckedHomeworksCountQuery : IRequest<IList<TeacherUncheckedHomeworksCountModel>>
{
}

public class TeacherUncheckedHomeworksCountModel
{
    public int TeacherId { get; set; }
    public int Count { get; set; }
}

public class GetTeachersUncheckedHomeworksCountQueryHandler : IRequestHandler<GetTeachersUncheckedHomeworksCountQuery,
    IList<TeacherUncheckedHomeworksCountModel>>
{
    private readonly IApplicationDbContext _context;

    public GetTeachersUncheckedHomeworksCountQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<TeacherUncheckedHomeworksCountModel>> Handle(GetTeachersUncheckedHomeworksCountQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _context.CompletedHomeworks
            .Where(ch => ch.Grade == null)
            .GroupBy(ch => ch.CheckingTeacherId)
            .Select(group => new TeacherUncheckedHomeworksCountModel
            {
                TeacherId = group.Key,
                Count = group.Count()
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}