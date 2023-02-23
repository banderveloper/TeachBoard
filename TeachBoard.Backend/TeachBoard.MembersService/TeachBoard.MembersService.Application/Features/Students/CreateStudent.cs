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
            var existingGroup = await _context.Groups.FindAsync(request.GroupId, cancellationToken);

            if (existingGroup is null)
                throw new NotFoundException
                {
                    Error = "group_not_found",
                    ErrorDescription = $"Group with id '{request.GroupId}' not found",
                    ReasonField = "groupId"
                };
        }
        

        // If student with given user id already exists - exception
        
        var existingStudent = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

        if (existingStudent is not null)
            throw new AlreadyExistsException
            {
                Error = "student_already_exists",
                ErrorDescription = $"Student with user id '{request.UserId}' already exists",
                ReasonField = "userId"
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