using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;
using TeachBoard.IdentityService.Domain.Enums;

namespace TeachBoard.IdentityService.Application.CQRS.Commands;

public class CreatePendingUserCommand : IRequest<RegisterCodeModel>
{
    public Role Role { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

// Command handling result, register code bound to created pending user
public class RegisterCodeModel
{
    public string RegisterCode { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

public class CreatePendingUserCommandHandler : IRequestHandler<CreatePendingUserCommand, RegisterCodeModel>
{
    private readonly IApplicationDbContext _context;
    private readonly PendingUserConfiguration _pendingConfiguration;

    private static readonly Random _random = new();

    public CreatePendingUserCommandHandler(IApplicationDbContext context, PendingUserConfiguration pendingConfiguration)
    {
        _context = context;
        _pendingConfiguration = pendingConfiguration;
    }

    public async Task<RegisterCodeModel> Handle(CreatePendingUserCommand request, CancellationToken cancellationToken)
    {
        // if phone or email entered - check it is not exists at db
        if (request.PhoneNumber is not null || request.Email is not null)
        {
            // get user with given email OR phone
            var existingPendingUser = await _context.Users
                .FirstOrDefaultAsync(user =>
                        user.PhoneNumber == request.PhoneNumber ||
                        string.Equals(user.Email.ToLower(), request.Email.ToLower()),
                    cancellationToken);

            if (existingPendingUser is not null)
            {
                var existingPropertyName = existingPendingUser.Email == request.Email ? "email" : "phoneNumber";
                var existingPropertyValue =
                    existingPendingUser.Email == request.Email ? request.Email : request.PhoneNumber;

                throw new ExpectedApiException
                {
                    ErrorCode = existingPropertyName + "_already_exists",
                    ReasonField = existingPropertyName,
                    PublicErrorMessage = $"User with given {existingPropertyName} already exists",
                    LogErrorMessage =
                        $"Create pending user error. User with given {existingPropertyName} [{existingPropertyValue}] already exists"
                };
            }
        }

        // if does not exists - create
        var pendingUser = new PendingUser
        {
            RegisterCode = GenerateRegisterCode(),
            ExpiresAt = DateTime.Now.AddHours(_pendingConfiguration.LifetimeHours),
            Role = request.Role,

            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Patronymic = request.Patronymic,
            HomeAddress = request.HomeAddress,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = request.DateOfBirth
        };

        _context.PendingUsers.Add(pendingUser);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterCodeModel
        {
            RegisterCode = pendingUser.RegisterCode,
            ExpiresAt = pendingUser.ExpiresAt
        };
    }

    // Generate random 8length register code for student
    private static string GenerateRegisterCode()
    {
        const int length = 8;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}