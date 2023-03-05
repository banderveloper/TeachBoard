using MediatR;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

// Query
public class GetStudentByIdQuery : IRequest<Student?>
{
    public int StudentId { get; set; }
}

// Handler
public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, Student?>
{
    private readonly IApplicationDbContext _context;

    public GetStudentByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Student?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students.FindAsync(new object[] {request.StudentId}, cancellationToken);
        
        return student;
    }
}