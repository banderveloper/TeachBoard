using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Extensions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Queries;

public class GetUserByCredentialsQuery : IRequest<User>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class GetUserByCredentialsQueryHandler : IRequestHandler<GetUserByCredentialsQuery, User>
{
    private readonly IApplicationDbContext _context;

    public GetUserByCredentialsQueryHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<User> Handle(GetUserByCredentialsQuery request, CancellationToken cancellationToken)
    {
        // get user by given username
        var user = await _context.Users
            .FirstOrDefaultAsync(
                user => string.Equals(user.UserName.ToLower(), request.UserName.ToLower()),
                cancellationToken);

        // if not found - exception
        if (user is null)
            throw new ExpectedApiException
            {
                ErrorCode = "user_not_found",
                ReasonField = "userName",
                PublicErrorMessage = "User with given username not found",
                LogErrorMessage = $"Get user by credentials error. User with username [{request.UserName}] not found"
            };

        // If password incorrect - exception
        if (request.Password.ToSha256() != user.PasswordHash)
            throw new ExpectedApiException
            {
                ErrorCode = "user_password_incorrect",
                PublicErrorMessage = "Incorrect password to given user",
                LogErrorMessage = $"Get user by credentials error. Wrong password to user [{user.UserName}]"
            };

        // if its ok - return user
        return user;
    }
}