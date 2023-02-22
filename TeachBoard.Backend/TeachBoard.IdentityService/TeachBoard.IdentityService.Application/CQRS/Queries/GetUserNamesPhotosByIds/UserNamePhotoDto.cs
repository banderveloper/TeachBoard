using AutoMapper;
using TeachBoard.IdentityService.Application.Mappings;
using TeachBoard.IdentityService.Domain.Entities;

namespace TeachBoard.IdentityService.Application.CQRS.Queries.GetUserNamesPhotosByIds;

public class UserNamePhotoDto : IMappable
{
    public int Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }

    public string? AvatarImagePath { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserNamePhotoDto>();
    }
}