using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Teachers;

// Query
public class GetAllTeachersQuery : IRequest<IList<Teacher>>
{
}

// Handler
public class GetAllTeachersQueryHandler : IRequestHandler<GetAllTeachersQuery, IList<Teacher>>
{
    private readonly IApplicationDbContext _context;

    public GetAllTeachersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Teacher>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
    {
        var teachers = await _context.Teachers.ToListAsync(cancellationToken);

        return teachers;
    }
}
