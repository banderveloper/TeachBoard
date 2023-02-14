using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Domain.Entities;
using TeachBoard.IdentityService.Application.Interfaces;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.CreatePendingUser;

public class CreatePendingUserCommandHandler : IRequestHandler<CreatePendingUserCommand, RegisterCodeModel>
{
    private readonly IApplicationDbContext _context;
    private static readonly Random _random = new();

    public CreatePendingUserCommandHandler(IApplicationDbContext context)
        => _context = context;


    public async Task<RegisterCodeModel> Handle(CreatePendingUserCommand request, CancellationToken cancellationToken)
    {
        // if phone or email entered - check it is not exists at db
        if (request.PhoneNumber is not null || request.Email is not null)
        {
            // get user with given email OR phone
            var existingPendingUser = await _context.PendingUsers
                .FirstOrDefaultAsync(pu => pu.Email == request.Email || pu.PhoneNumber == request.PhoneNumber,
                    cancellationToken);

            // if found - exception
            if (existingPendingUser is not null)
                throw new AlreadyExistsException
                {
                    Error =
                        existingPendingUser.Email == request.Email ? "email_already_exists" : "phone_already_exists",
                    ErrorDescription = "Pending user with given property already exists"
                };
        }

        // if does not exists - create
        var pendingUser = new PendingUser
        {
            RegisterCode = GenerateRegisterCode(),
            ExpiresAt = DateTime.Now.AddDays(3),
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