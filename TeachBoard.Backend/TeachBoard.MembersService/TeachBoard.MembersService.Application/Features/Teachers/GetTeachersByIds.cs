using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Teachers.Common;
using TeachBoard.MembersService.Application.Interfaces;

namespace TeachBoard.MembersService.Application.Features.Teachers;

public class GetTeachersByIdsQuery : IRequest<TeachersListModel>
{
    public IList<int> Ids { get; set; }
}

public class GetTeachersByIdsQueryHandler : IRequestHandler<GetTeachersByIdsQuery, TeachersListModel>
{
    private readonly IApplicationDbContext _context;

    public GetTeachersByIdsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TeachersListModel> Handle(GetTeachersByIdsQuery request, CancellationToken cancellationToken)
    {
        var teachers = await _context.Teachers
            .Where(t => request.Ids.Contains(t.Id))
            .ToListAsync(cancellationToken);

        return new TeachersListModel { Teachers = teachers };
    }
}