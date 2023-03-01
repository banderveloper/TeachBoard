using MediatR;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;

namespace TeachBoard.MembersService.Application.Features.Students;

public class SetStudentGroupCommand : IRequest
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }
}

public class SetStudentGroupCommandHandler : IRequestHandler<SetStudentGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public SetStudentGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SetStudentGroupCommand request, CancellationToken cancellationToken)
    {
        // Find student by student, if not exists - error
        var existingStudent = await _context.Students.FindAsync(request.StudentId, cancellationToken);
        if (existingStudent is null)
            throw new NotFoundException
            {
                Error = "student_not_found",
                ErrorDescription = $"Student with id '{request.StudentId}' not found",
                ReasonField = "studentId"
            };

        // Find existing group, if not exists - error
        var existingGroup = await _context.Groups.FindAsync(request.GroupId, cancellationToken);
        if(existingGroup is null)
            throw new NotFoundException
            {
                Error = "group_not_found",
                ErrorDescription = $"Group with id '{request.StudentId}' not found",
                ReasonField = "groupId"
            };

        existingStudent.GroupId = request.GroupId;
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}