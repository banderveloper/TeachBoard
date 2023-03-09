using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.IdentityService.Application.Features.Commands;
using TeachBoard.IdentityService.Application.Mappings;

namespace TeachBoard.IdentityService.WebApi.Models.User;

public class ApprovePendingUserRequestModel : IMappable
{
    [Required(ErrorMessage = "Register code is required. It was given by administrator")] 
    public string RegisterCode { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Username is required.")] 
    [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{5,17}$", 
        ErrorMessage = "Username is invalid. Use only latin letters, numbers and _, length 6-18 symbols")]
    public string UserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    public string PasswordHash { get; set; } = string.Empty;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApprovePendingUserRequestModel, ApprovePendingUserCommand>();
    }
}