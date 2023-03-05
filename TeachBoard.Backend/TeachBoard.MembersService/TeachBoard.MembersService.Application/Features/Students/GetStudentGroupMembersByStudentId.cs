using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Students.Common;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

public class GetStudentGroupMembersByStudentIdQuery : IRequest<StudentsListModel>
{
    public int StudentId { get; set; }
}

public class
    GetStudentGroupMembersByStudentIdQueryHandler : IRequestHandler<GetStudentGroupMembersByStudentIdQuery,
        StudentsListModel>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupMembersByStudentIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentsListModel> Handle(GetStudentGroupMembersByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        // Searching for given student by id
        var student = await _context.Students.FindAsync(new object[] { request.StudentId }, cancellationToken);

        if (student is null)
            return new StudentsListModel { Students = new List<Student>() };
        
        // Searching for student group members
        var groupMembers = await _context.Students
            .Where(st => st.GroupId == student.GroupId)
            .ToListAsync(cancellationToken);

        return new StudentsListModel { Students = groupMembers };
    }
}