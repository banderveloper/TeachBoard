using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Groups;

// Command
public class CreateGroupCommand : IRequest<Group>
{
    public string Name { get; set; } = string.Empty;
}

// Handler
public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Group>
{
    private readonly IApplicationDbContext _context;

    public CreateGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Group> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var existingGroup =
            await _context.Groups.FirstOrDefaultAsync(g => string.Equals(g.Name.ToLower(), request.Name.ToLower()),
                cancellationToken);

        if (existingGroup is not null)
            throw new ExpectedApiException
            {
                ErrorCode = "group_already_exists",
                PublicErrorMessage = "Group with given name already exists",
                LogErrorMessage = $"CreateGroup command error. Group with name {request.Name} already exists",
                ReasonField = "name"
            };

        var newGroup = new Group
        {
            Name = request.Name
        };
        _context.Groups.Add(newGroup);
        await _context.SaveChangesAsync(cancellationToken);

        return newGroup;
    }
}