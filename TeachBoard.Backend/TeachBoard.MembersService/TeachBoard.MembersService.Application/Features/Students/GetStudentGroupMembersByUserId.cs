using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Students.Common;
using TeachBoard.MembersService.Application.Interfaces;

namespace TeachBoard.MembersService.Application.Features.Students;

public class GetStudentGroupMembersByUserIdQuery : IRequest<StudentsListModel>
{
    public int UserId { get; set; }
}

public class GetStudentGroupMembersByUserIdQueryHandler
    : IRequestHandler<GetStudentGroupMembersByUserIdQuery, StudentsListModel>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupMembersByUserIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentsListModel> Handle(GetStudentGroupMembersByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var studentByUserId = await _context.Students
            .FirstOrDefaultAsync(s => s.UserId == request.UserId,
                cancellationToken);

        if (studentByUserId is null)
            throw new NotFoundException
            {
                Error = "student_not_found",
                ErrorDescription = $"Student with user id '{request.UserId}' not found",
                ReasonField = "userId"
            };

        if (studentByUserId.GroupId is null)
            throw new NotFoundException
            {
                Error = "group_not_found",
                ErrorDescription = $"Student with id '{studentByUserId.Id}' does not belong to any group",
                ReasonField = "groupId"
            };

        var studentGroupMembers = await _context.Students
            .Where(s => s.GroupId == studentByUserId.GroupId)
            .ToListAsync(cancellationToken);

        if (studentGroupMembers.Count == 0)
            throw new NotFoundException
            {
                Error = "group_members_not_found",
                ErrorDescription = $"Student with id '{studentByUserId.UserId}' does not have group so-members"
            };

        return new StudentsListModel
        {
            Students = studentGroupMembers
        };
    }
}