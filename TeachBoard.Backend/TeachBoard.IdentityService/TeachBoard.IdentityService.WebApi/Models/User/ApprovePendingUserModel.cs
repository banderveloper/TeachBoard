using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.IdentityService.Application.CQRS.Commands.ApprovePendingUser;
using TeachBoard.IdentityService.Application.Mappings;

namespace TeachBoard.IdentityService.WebApi.Models.User;

public class ApprovePendingUserModel : IMappable
{
    [Required(ErrorMessage = "Register code is required. It was given by administrator")] 
    public string RegisterCode { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Username is required.")] 
    [RegularExpression(@"^[a-zA-Z0-9](_(?!(\.|_))|\.(?!(_|\.))|[a-zA-Z0-9]){6,18}[a-zA-Z0-9]$", 
        ErrorMessage = "Username is invalid. Use only latin letters, numbers and _, length 6-18 symbols")]
    public string UserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    public string PasswordHash { get; set; } = string.Empty;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApprovePendingUserModel, ApprovePendingUserCommand>();
    }
}