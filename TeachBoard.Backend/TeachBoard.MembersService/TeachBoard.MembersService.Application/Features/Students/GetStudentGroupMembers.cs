using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Students.Common;
using TeachBoard.MembersService.Application.Interfaces;

namespace TeachBoard.MembersService.Application.Features.Students;

public class GetStudentGroupMembersQuery : IRequest<StudentsListModel>
{
    public int StudentId { get; set; }
}

public class GetStudentGroupMembersQueryHandler : IRequestHandler<GetStudentGroupMembersQuery, StudentsListModel>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupMembersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentsListModel> Handle(GetStudentGroupMembersQuery request,
        CancellationToken cancellationToken)
    {
        // Searching for given student by id
        var student = await _context.Students.FindAsync(request.StudentId, cancellationToken);

        if (student is null)
            throw new NotFoundException
            {
                Error = "student_not_found",
                ErrorDescription = $"Student with id {request.StudentId} not found",
                ReasonField = "id"
            };

        // Searching for student group members
        var groupMembers = await _context.Students
            .Where(st => st.GroupId == student.GroupId)
            .ToListAsync(cancellationToken);

        return new StudentsListModel { Students = groupMembers };
    }
}