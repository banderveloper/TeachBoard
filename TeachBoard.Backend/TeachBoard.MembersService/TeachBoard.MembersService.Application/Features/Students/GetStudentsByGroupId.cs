using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

// Query
public class GetStudentsByGroupIdQuery : IRequest<IList<Student>>
{
    public int GroupId { get; set; }
}

// Handler
public class GetStudentsByGroupIdQueryHandler : IRequestHandler<GetStudentsByGroupIdQuery, IList<Student>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentsByGroupIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Student>> Handle(GetStudentsByGroupIdQuery request, CancellationToken cancellationToken)
    {
        var students = await _context.Students
            .Where(s => s.GroupId == request.GroupId)
            .ToListAsync(cancellationToken);

        return students;
    }
}

