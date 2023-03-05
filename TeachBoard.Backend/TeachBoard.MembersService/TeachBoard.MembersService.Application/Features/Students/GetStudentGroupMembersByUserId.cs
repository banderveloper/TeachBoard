using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

public class GetStudentGroupMembersByUserIdQuery : IRequest<IList<Student>>
{
    public int UserId { get; set; }
}

public class GetStudentGroupMembersByUserIdQueryHandler
    : IRequestHandler<GetStudentGroupMembersByUserIdQuery, IList<Student>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupMembersByUserIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Student>> Handle(GetStudentGroupMembersByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var studentByUserId = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == request.UserId,
                cancellationToken);

        if (studentByUserId is null)
            return new List<Student>();

        var studentGroupMembers = await _context.Students
            .Where(s => s.GroupId == studentByUserId.GroupId)
            .ToListAsync(cancellationToken);

        return studentGroupMembers;
    }
}