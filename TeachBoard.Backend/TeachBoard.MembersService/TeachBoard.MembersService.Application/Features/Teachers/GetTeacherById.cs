using MediatR;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Teachers;

// Query
public class GetTeacherByIdQuery : IRequest<Teacher>
{
    public int TeacherId { get; set; }
}

// Handler
public class GetTeacherByIdQueryHandler : IRequestHandler<GetTeacherByIdQuery, Teacher>
{
    private readonly IApplicationDbContext _context;

    public GetTeacherByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Teacher> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
    {
        var teacher = await _context.Teachers.FindAsync(request.TeacherId, cancellationToken);

        if (teacher is null)
            throw new NotFoundException
            {
                Error = "teacher_not_found",
                ErrorDescription = $"Teacher with id {request.TeacherId} not found",
                ReasonField = "id"
            };

        return teacher;
    }
}