using AutoMapper;
using TeachBoard.IdentityService.Application.Features.Commands;
using TeachBoard.IdentityService.Application.Mappings;

namespace TeachBoard.IdentityService.WebApi.Models.User;

public class UpdateUserAvatarRequestModel : IMappable
{
    public int UserId { get; set; }
    public string AvatarImagePath { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserAvatarRequestModel, UpdateUserAvatarImagePathCommand>();
    }
}