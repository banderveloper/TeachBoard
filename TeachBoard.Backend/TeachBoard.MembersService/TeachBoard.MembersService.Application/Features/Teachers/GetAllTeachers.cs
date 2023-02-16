using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Teachers;

// Query
public class GetAllTeachersQuery : IRequest<TeachersListModel>
{
}

// Handler
public class GetAllTeachersQueryHandler : IRequestHandler<GetAllTeachersQuery, TeachersListModel>
{
    private readonly IApplicationDbContext _context;

    public GetAllTeachersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TeachersListModel> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
    {
        var teachers = await _context.Teachers.ToListAsync(cancellationToken);

        if (teachers.Count == 0)
            throw new NotFoundException
            {
                Error = "teachers_not_found",
                ErrorDescription = "Teachers not found"
            };

        return new TeachersListModel
        {
            Teachers = teachers
        };
    }
}

public class TeachersListModel
{
    public IList<Teacher> Teachers { get; set; }
}