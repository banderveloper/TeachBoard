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
        var existingStudent = await _context.Students.FindAsync(new object[] { request.StudentId }, cancellationToken);
        if (existingStudent is null)
            throw new ExpectedApiException
            {
                ErrorCode = "student_not_found",
                PublicErrorMessage = "Student not found",
                LogErrorMessage = $"SetStudentGroup command error. Student with id {request.StudentId} not found"
            };

        // Find existing group, if not exists - error
        var existingGroup = await _context.Groups.FindAsync(new object[] { request.GroupId }, cancellationToken);
        if (existingGroup is null)
            throw new ExpectedApiException
            {
                ErrorCode = "group_not_found",
                PublicErrorMessage = "Group not found",
                LogErrorMessage = $"SetStudentGroup command error. Group with id {request.GroupId} not found"
            };

        existingStudent.GroupId = request.GroupId;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}