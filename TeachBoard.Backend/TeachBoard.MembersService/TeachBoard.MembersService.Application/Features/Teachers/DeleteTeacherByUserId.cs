using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;

namespace TeachBoard.MembersService.Application.Features.Teachers;

// Command
public class DeleteTeacherByUserIdCommand : IRequest<bool>
{
    public int UserId { get; set; }
}

// Handler
public class DeleteTeacherByUserIdCommandHandler : IRequestHandler<DeleteTeacherByUserIdCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteTeacherByUserIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTeacherByUserIdCommand request, CancellationToken cancellationToken)
    {
        var teachers = await _context.Teachers
            .Where(t => t.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        if (teachers.Count == 0)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.TeacherNotFound,
                PublicErrorMessage = "Teacher cannot be deleted because it does not exists",
                LogErrorMessage = $"DeleteTeacherByUserId error. Teacher with user id {request.UserId} does not exists"
            };
        
        _context.Teachers.RemoveRange(teachers);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}