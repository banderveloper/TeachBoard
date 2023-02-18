using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Extensions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.ApprovePendingUser;

// Transform pending user to real user using registration with given registration code and new username and pass
public class ApprovePendingUserCommandHandler : IRequestHandler<ApprovePendingUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public ApprovePendingUserCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<User> Handle(ApprovePendingUserCommand request, CancellationToken cancellationToken)
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

        // if pending user expired - exception
        if (pendingUser.ExpiresAt < DateTime.Now)
            throw new DataExpiredException
            {
                Error = "pending_user_expired",
                ErrorDescription = $"Pending user expired {pendingUser.ExpiresAt.ToUniversalTime()}"
            };
        
        
        // trying to find existing user with given login
        var userByUsername = await _context.Users
            .FirstOrDefaultAsync(user => user.UserName == request.UserName,
                cancellationToken);

        // if it is exists - already exists exception
        if (userByUsername is not null)
            throw new AlreadyExistsException
            {
                Error = "username_already_exists",
                ErrorDescription = $"User with username {request.UserName} already exists"
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

        return newUser;
    }
}