using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

public class GetStudentGroupMembersByStudentIdQuery : IRequest<IList<Student>>
{
    public int StudentId { get; set; }
}

public class
    GetStudentGroupMembersByStudentIdQueryHandler : IRequestHandler<GetStudentGroupMembersByStudentIdQuery,
        IList<Student>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupMembersByStudentIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Student>> Handle(GetStudentGroupMembersByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        // Searching for given student by id
        var student = await _context.Students.FindAsync(new object[] { request.StudentId }, cancellationToken);

        if (student is null)
            return new List<Student>();
        
        // Searching for student group members
        var groupMembers = await _context.Students
            .Where(st => st.GroupId == student.GroupId)
            .ToListAsync(cancellationToken);

        return groupMembers;
    }
}