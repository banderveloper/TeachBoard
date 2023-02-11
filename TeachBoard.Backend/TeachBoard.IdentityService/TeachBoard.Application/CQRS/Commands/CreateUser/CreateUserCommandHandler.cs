using MediatR;
using TeachBoard.Application.Interfaces;

namespace TeachBoard.Application.CQRS.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUserModel>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreatedUserModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        
    }
    
    
}