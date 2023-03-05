using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Teachers;

// Command
public class CreateTeacherCommand : IRequest<Teacher>
{
    public int UserId { get; set; }
}

// Handler
public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, Teacher>
{
    private readonly IApplicationDbContext _context;

    public CreateTeacherCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Teacher> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {
        // if teacher with given user id exists - exception
        var existingTeacher = await _context.Teachers
            .FirstOrDefaultAsync(t => t.UserId == request.UserId, cancellationToken);

        if (existingTeacher is not null)
            throw new ExpectedApiException
            {
                ErrorCode = "teacher_already_exists",
                PublicErrorMessage = "One teacher is already bound to given user profile",
                LogErrorMessage = $"CreateTeacher command error. Teacher with user id {request.UserId} already exists"
            };
        
        // if ok - create
        var teacher = new Teacher { UserId = request.UserId };
        
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync(cancellationToken);

        return teacher;
    }
}

