using MediatR;

namespace TeachBoard.IdentityService.Application.CQRS.Queries.GetUserNamesPhotosByIds;

public class GetUserNamesPhotosByIdsQuery : IRequest<UsersNamePhotoListModel>
{
    public List<int> Ids { get; set; }
}