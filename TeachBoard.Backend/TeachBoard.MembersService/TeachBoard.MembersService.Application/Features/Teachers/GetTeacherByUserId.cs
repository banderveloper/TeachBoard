using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Teachers;

public class GetTeacherByUserIdQuery : IRequest<Teacher?>
{
    public int UserId { get; set; }
}

public class GetTeacherByUserIdQueryHandler : IRequestHandler<GetTeacherByUserIdQuery, Teacher?>
{
    private readonly IApplicationDbContext _context;

    public GetTeacherByUserIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Teacher?> Handle(GetTeacherByUserIdQuery request, CancellationToken cancellationToken) =>
        await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == request.UserId, cancellationToken);
}