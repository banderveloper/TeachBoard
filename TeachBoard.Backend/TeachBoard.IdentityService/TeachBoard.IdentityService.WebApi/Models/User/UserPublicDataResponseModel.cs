using AutoMapper;
using TeachBoard.IdentityService.Application.Mappings;
using TeachBoard.IdentityService.Domain.Enums;

namespace TeachBoard.IdentityService.WebApi.Models.User;

public class UserPublicDataResponseModel : IMappable
{
    public string UserName { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Unspecified;
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    
    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    public string? AvatarImagePath { get; set; }

    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.User, UserPublicDataResponseModel>();
    }
}