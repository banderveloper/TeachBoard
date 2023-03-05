using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;

namespace TeachBoard.MembersService.Application.Features.Students;

// Command
public class DeleteStudentByUserIdCommand : IRequest<bool>
{
    public int UserId { get; set; }
}

// Handler
public class DeleteStudentByUserIdCommandHandler : IRequestHandler<DeleteStudentByUserIdCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentByUserIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteStudentByUserIdCommand request, CancellationToken cancellationToken)
    {
        var students = await _context.Students
            .Where(s => s.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        if (students.Count == 0)
            throw new ExpectedApiException
            {
                ErrorCode = "student_not_found",
                PublicErrorMessage = "Student cannot be deleted because it does not exists",
                LogErrorMessage = $"DeleteStudentByUserId error. Student with user id {request.UserId} does not exists"
            };
        
        _context.Students.RemoveRange(students);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}