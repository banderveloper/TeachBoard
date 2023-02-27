using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

public class GetStudentByUserIdQuery : IRequest<Student>
{
    public int UserId { get; set; }
}

public class GetStudentByUserIdQueryHandler : IRequestHandler<GetStudentByUserIdQuery, Student>
{
    private readonly IApplicationDbContext _context;

    public GetStudentByUserIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Student> Handle(GetStudentByUserIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

        if (student is null)
            throw new NotFoundException
            {
                Error = "student_not_found",
                ErrorDescription = $"Student with user id '{request.UserId}' not found",
                ReasonField = "userId"
            };

        return student;
    }
}