using MediatR;
using TeachBoard.IdentityService.Domain.Entities;
using TeachBoard.IdentityService.Application.Interfaces;

namespace TeachBoard.IdentityService.Application.CQRS.Commands.CreatePendingUser;

public class CreatePendingUserCommandHandler : IRequestHandler<CreatePendingUserCommand, RegisterCodeModel>
{
    private readonly IApplicationDbContext _context;
    private static readonly Random _random = new Random();

    public CreatePendingUserCommandHandler(IApplicationDbContext context)
        => _context = context;


    public async Task<RegisterCodeModel> Handle(CreatePendingUserCommand request, CancellationToken cancellationToken)
    {
        var pendingUser = new PendingUser
        {
            RegisterCode = GenerateRegisterCode(),
            ExpiresAt = DateTime.Now.AddDays(3),
            
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