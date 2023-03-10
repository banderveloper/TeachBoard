using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Features.Queries.Common;
using TeachBoard.IdentityService.Application.Interfaces;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.Features.Queries;

public class GetUsersPresentationDataByPartialNameQuery : IRequest<IList<UserPresentationDataModel>>
{
    public string PartialName { get; set; }
}

public class GetUsersPresentationDataByPartialNameQueryHandler : IRequestHandler<
    GetUsersPresentationDataByPartialNameQuery, IList<UserPresentationDataModel>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersPresentationDataByPartialNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<UserPresentationDataModel>> Handle(GetUsersPresentationDataByPartialNameQuery request,
        CancellationToken cancellationToken)
    {
        if (request.PartialName.Length < 3)
            return new List<UserPresentationDataModel>();
        
        var predicate = PredicateBuilder.New<User>();

        // todo toLower
        
        predicate = predicate.And(user =>
            user.FirstName.Contains(request.PartialName) ||
            user.LastName.Contains(request.PartialName) ||
            user.Patronymic.Contains(request.PartialName));

        var users = await _context.Users.AsExpandable()
            .Where(predicate)
            .Select(user => new UserPresentationDataModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                AvatarImagePath = user.AvatarImagePath
            })
            .ToListAsync(cancellationToken);
        
        return users;
    }
}