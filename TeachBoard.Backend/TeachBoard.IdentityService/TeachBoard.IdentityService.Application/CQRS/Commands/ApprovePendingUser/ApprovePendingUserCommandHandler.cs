using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Extensions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.ApprovePendingUser;

// Transform pending user to real user using registration with given registration code and new username and pass
public class ApprovePendingUserCommandHandler : IRequestHandler<ApprovePendingUserCommand>
{
    private readonly IApplicationDbContext _context;

    public ApprovePendingUserCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Unit> Handle(ApprovePendingUserCommand request, CancellationToken cancellationToken)
    {
        // Get pending user by registration code
        var pendingUser = await _context.PendingUsers
            .FirstOrDefaultAsync(pu => pu.RegisterCode == request.RegisterCode,
                cancellationToken);

        // If not found - exception
        if (pendingUser is null)
            throw new NotFoundException
            {
                Error = "register_code_not_found",
                ErrorDescription = $"Pending user approval error. Register code {request.RegisterCode} not found"
            };
        
        // Create user account from pending
        var newUser = new User
        {
            UserName = request.UserName,
            PasswordHash = request.PasswordHash.ToSha256(),

            FirstName = pendingUser.FirstName,
            LastName = pendingUser.LastName,
            Patronymic = pendingUser.Patronymic,
            Email = pendingUser.Email,
            Role = pendingUser.Role,
            HomeAddress = pendingUser.HomeAddress,
            PhoneNumber = pendingUser.PhoneNumber,
            DateOfBirth = pendingUser.DateOfBirth
        };
        _context.Users.Add(newUser);

        // Remove pending user because we created real user from it
        _context.PendingUsers.Remove(pendingUser);
        
        // Save it all
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}