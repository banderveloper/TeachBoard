using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Extensions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Commands;

// Transform pending user to real user using registration with given registration code and new username and pass

public class ApprovePendingUserCommand : IRequest<User>
{
    public string RegisterCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}

public class ApprovePendingUserCommandHandler : IRequestHandler<ApprovePendingUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public ApprovePendingUserCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<User> Handle(ApprovePendingUserCommand request, CancellationToken cancellationToken)
    {
        // Get pending user by registration code
        var pendingUser = await _context.PendingUsers
            .FirstOrDefaultAsync(
                pu => string.Equals(pu.RegisterCode.ToLower(), request.RegisterCode.ToLower()),
                cancellationToken);

        // If not found - exception
        if (pendingUser is null)
            throw new ExpectedApiException
            {
                ErrorCode = "pending_user_not_found",
                ReasonField = "registerCode",
                PublicErrorMessage = "Pending user with given register code does not exists",
                LogErrorMessage =
                    $"Approve pending error. Pending user with register code [{request.RegisterCode}] not found"
            };

        // if pending user expired - exception
        if (DateTime.Now > pendingUser.ExpiresAt)
            throw new ExpectedApiException
            {
                ErrorCode = "pending_user_expired",
                PublicErrorMessage = "Pending user registration time expired",
                LogErrorMessage =
                    $"Approve pending error. Requested pending user expired at [{pendingUser.ExpiresAt.ToUniversalTime()}]"
            };

        // trying to find existing user with given login
        var userByUsername = await _context.Users
            .FirstOrDefaultAsync(
                user => string.Equals(user.UserName.ToLower(), request.UserName.ToLower()),
                cancellationToken);

        // if it is exists - already exists exception
        if (userByUsername is not null)
            throw new ExpectedApiException
            {
                ErrorCode = "user_already_exists",
                ReasonField = "userName",
                PublicErrorMessage = "User with given username already exists",
                LogErrorMessage = $"Approve pending error. Username [{request.UserName}] already exists"
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