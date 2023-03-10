using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using TeachBoard.IdentityService.Application.Converters;
using TeachBoard.IdentityService.Application.Features.Commands;
using TeachBoard.IdentityService.Application.Mappings;
using TeachBoard.IdentityService.Domain.Enums;

namespace TeachBoard.IdentityService.WebApi.Models.User;

public class UpdateUserRequestModel : IMappable
{
    public int Id { get; set; }

    [Required] public string UserName { get; set; } = string.Empty;
    
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string Patronymic { get; set; }

    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserRequestModel, UpdateUserCommand>();
    }
}