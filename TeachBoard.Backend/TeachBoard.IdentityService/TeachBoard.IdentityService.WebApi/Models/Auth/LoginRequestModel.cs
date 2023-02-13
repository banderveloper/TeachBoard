using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.IdentityService.Application.CQRS.Queries.GetUserByCredentials;
using TeachBoard.IdentityService.Application.Mappings;

namespace TeachBoard.IdentityService.WebApi.Models.Auth;

public class LoginRequestModel : IMappable
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<LoginRequestModel, GetUserByCredentialsQuery>();
    }
}