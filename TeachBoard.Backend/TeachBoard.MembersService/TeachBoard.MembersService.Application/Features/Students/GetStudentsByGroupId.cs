using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

// Query
public class GetStudentsByGroupIdQuery : IRequest<StudentsListModel>
{
    public int GroupId { get; set; }
}

// Handler
public class GetStudentsByGroupIdQueryHandler : IRequestHandler<GetStudentsByGroupIdQuery, StudentsListModel>
{
    private readonly IApplicationDbContext _context;

    public GetStudentsByGroupIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentsListModel> Handle(GetStudentsByGroupIdQuery request, CancellationToken cancellationToken)
    {
        var students = await _context.Students
            .Where(s => s.GroupId == request.GroupId)
            .ToListAsync(cancellationToken);

        if (students.Count == 0)
            throw new NotFoundException
            {
                Error = "students_not_found",
                ErrorDescription = $"Students with group id {request.GroupId} not found",
                ReasonField = "groupId"
            };
        
        return new StudentsListModel { Students = students };
    }
}

// Return model
public class StudentsListModel
{
    public IList<Student> Students { get; set; }
}