using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

// Command
public class CreateStudentCommand : IRequest<Student>
{
    public int UserId { get; set; }
    public int? GroupId { get; set; }
}

// Handler
public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Student>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Student> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        // If group does not exists - exception
        if (request.GroupId != null)
        {
            var existingGroup = await _context.Groups.FindAsync(new object[] { request.GroupId }, cancellationToken);

            if (existingGroup is null)
                throw new ExpectedApiException
                {
                    ErrorCode = ErrorCode.GroupNotFound,
                    PublicErrorMessage = $"Group not found",
                    LogErrorMessage = $"CreateStudent command error. Group with id {request.GroupId} not found"
                };
        }
        
        // If student with given user id already exists - exception

        var existingStudent = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

        if (existingStudent is not null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.StudentAlreadyExists,
                PublicErrorMessage = "One student is already bound to given user profile",
                LogErrorMessage = $"CreateStudent error. Student with user id {request.UserId} already exists"
            };

        // if ok - create student in db

        var student = new Student
        {
            UserId = request.UserId,
            GroupId = request.GroupId
        };
        _context.Students.Add(student);
        await _context.SaveChangesAsync(cancellationToken);

        return student;
    }
}