using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Entities;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class GetTeacherUncheckedHomeworksQuery : IRequest<IList<CompletedHomework>>
{
    public int TeacherId { get; set; }
}

public class
    GetTeacherUncheckedHomeworksQueryHandler : IRequestHandler<GetTeacherUncheckedHomeworksQuery,
        IList<CompletedHomework>>
{
    private readonly IApplicationDbContext _context;

    public GetTeacherUncheckedHomeworksQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<CompletedHomework>> Handle(GetTeacherUncheckedHomeworksQuery request,
        CancellationToken cancellationToken) =>
        await _context.CompletedHomeworks.Where(h =>
                h.CheckingTeacherId == request.TeacherId && h.Grade == null)
            .ToListAsync(cancellationToken);
}