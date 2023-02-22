using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.IdentityService.Application.Exceptions;
using TeachBoard.IdentityService.Application.Interfaces;

namespace TeachBoard.IdentityService.Application.CQRS.Queries.GetUserNamesPhotosByIds;

public class
    GetUserNamesPhotosByIdsQueryHandler : IRequestHandler<GetUserNamesPhotosByIdsQuery, UsersNamePhotoListModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserNamesPhotosByIdsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UsersNamePhotoListModel> Handle(GetUserNamesPhotosByIdsQuery request,
        CancellationToken cancellationToken)
    {
        var userDtos = await _context.Users
            .Where(user => request.Ids.Contains(user.Id))
            .ProjectTo<UserNamePhotoDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (userDtos.Count == 0)
            throw new NotFoundException
            {
                Error = "users_not_found",
                // users with ids '1, 2, 3' not found
                ErrorDescription = $"Users with ids '{string.Join(", ", request.Ids)}' not found"
            };
        
        return new UsersNamePhotoListModel
        {
            Users = userDtos
        };
    }
}