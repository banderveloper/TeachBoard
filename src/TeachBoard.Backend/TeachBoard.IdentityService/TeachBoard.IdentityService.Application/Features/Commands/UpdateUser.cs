using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.Features.Commands;

public class UpdateUserCommand : IRequest<User>
{
    public int Id { get; set; }
    public string UserName { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }

    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsTracking()
            .FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);

        if (user is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.UserNotFound,
                PublicErrorMessage = "User not found",
                ReasonField = "id",
                LogErrorMessage = $"UpdateUserCommand error. User with id [{request.Id}] not found"
            };

        user.UserName = request.UserName;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Patronymic = request.Patronymic;
        user.PhoneNumber = request.PhoneNumber;
        user.HomeAddress = request.HomeAddress;
        user.Email = request.Email;
        user.DateOfBirth = request.DateOfBirth;

        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }
}