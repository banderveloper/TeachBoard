using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Features.Queries.Common;
using TeachBoard.IdentityService.Application.Interfaces;

namespace TeachBoard.IdentityService.Application.Features.Queries;

// Get users presentation data (id, full name, avatar image) by list of users ids

public class GetUsersPresentationDataByIdsQuery : IRequest<IList<UserPresentationDataModel>>
{
    public IList<int> Ids { get; set; }
}



public class GetUserNamesPhotosByIdsQueryHandler : IRequestHandler<GetUsersPresentationDataByIdsQuery,
    IList<UserPresentationDataModel>>
{
    private readonly IApplicationDbContext _context;

    public GetUserNamesPhotosByIdsQueryHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<IList<UserPresentationDataModel>> Handle(GetUsersPresentationDataByIdsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .Where(user => request.Ids.Contains(user.Id))
            .Select(user => new UserPresentationDataModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                AvatarImagePath = user.AvatarImagePath
            })
            .ToListAsync(cancellationToken);
    }
}