using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students;

public class GetStudentGroupByUserIdQuery : IRequest<Group?>
{
    public int UserId { get; set; }
}

public class GetStudentGroupByUserIdQueryHandler : IRequestHandler<GetStudentGroupByUserIdQuery, Group?>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupByUserIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Group?> Handle(GetStudentGroupByUserIdQuery request, CancellationToken cancellationToken)
    {
        var studentByUserId = await _context.Students
            .Include(s => s.Group)
            .FirstOrDefaultAsync(s => s.UserId == request.UserId,
                cancellationToken);
        
        return studentByUserId?.Group;
    }
}