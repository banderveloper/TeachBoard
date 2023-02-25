using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class GetHomeworksByGroupIdQuery : IRequest<HomeworksListModel>
{
    public int GroupId { get; set; }
}

public class GetHomeworksByGroupIdQueryHandler : IRequestHandler<GetHomeworksByGroupIdQuery, HomeworksListModel>
{
    private readonly IApplicationDbContext _context;

    public GetHomeworksByGroupIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HomeworksListModel> Handle(GetHomeworksByGroupIdQuery request,
        CancellationToken cancellationToken)
    {
        var groupHomeworks = await _context.Homeworks
            .Where(h => h.GroupId == request.GroupId)
            .ToListAsync(cancellationToken);

        if (groupHomeworks.Count == 0)
            throw new NotFoundException
            {
                Error = "homeworks_not_found",
                ErrorDescription = $"Homeworks for group with id '{request.GroupId}' not found",
                ReasonField = "groupId"
            };

        return new HomeworksListModel { Homeworks = groupHomeworks };
    }
}