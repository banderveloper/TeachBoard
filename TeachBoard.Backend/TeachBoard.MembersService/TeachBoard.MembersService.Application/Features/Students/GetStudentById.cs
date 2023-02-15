using MediatR;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

// Query
public class GetStudentByIdQuery : IRequest<Student>
{
    public int StudentId { get; set; }
}

// Handler
public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, Student>
{
    private readonly IApplicationDbContext _context;

    public GetStudentByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Student> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students.FindAsync(request.StudentId, cancellationToken);

        if (student is null)
            throw new NotFoundException
            {
                Error = "student_not_found",
                ErrorDescription = $"Student with id {request.StudentId} not found",
                ReasonField = "id"
            };

        return student;
    }
}