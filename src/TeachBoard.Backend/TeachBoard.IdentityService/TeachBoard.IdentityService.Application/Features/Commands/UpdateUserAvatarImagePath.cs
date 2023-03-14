using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.Features.Commands;

public class UpdateUserAvatarImagePathCommand : IRequest<User>
{
    public int UserId { get; set; }
    public string AvatarImagePath { get; set; } = string.Empty;
}

public class UpdateUserAvatarImagePathCommandHandler : IRequestHandler<UpdateUserAvatarImagePathCommand, User>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserAvatarImagePathCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(UpdateUserAvatarImagePathCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.AsTracking()
            .FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.UserNotFound,
                PublicErrorMessage = "User not found",
                LogErrorMessage = $"UpdateUserAvatarImagePathCommand error. User with id [{request.UserId}] not found"
            };

        user.AvatarImagePath = request.AvatarImagePath;
        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }
}