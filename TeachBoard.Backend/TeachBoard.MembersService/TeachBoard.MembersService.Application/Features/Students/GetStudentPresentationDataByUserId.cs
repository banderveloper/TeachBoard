using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using Group = TeachBoard.MembersService.Domain.Entities.Group;

namespace TeachBoard.MembersService.Application.Features.Students;

public class GetStudentPresentationDataByUserIdQuery : IRequest<StudentPresentationDataModel?>
{
    public int UserId { get; set; }
}

public class StudentPresentationDataModel
{
    public int StudentId { get; set; }
    public int UserId { get; set; }
    public Group? Group { get; set; }
}

public class
    GetStudentPresentationDataByUserIdQueryHandler : IRequestHandler<GetStudentPresentationDataByUserIdQuery,
        StudentPresentationDataModel?>
{
    private readonly IApplicationDbContext _context;

    public GetStudentPresentationDataByUserIdQueryHandler(IApplicationDbContext context) =>
        _context = context;


    public async Task<StudentPresentationDataModel?> Handle(GetStudentPresentationDataByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Include(student => student.Group)
            .FirstOrDefaultAsync(student => student.UserId == request.UserId, cancellationToken);

        if (student is null)
            return null;

        return new StudentPresentationDataModel
        {
            StudentId = student.Id,
            UserId = student.UserId,
            Group = student.Group
        };
    }
}