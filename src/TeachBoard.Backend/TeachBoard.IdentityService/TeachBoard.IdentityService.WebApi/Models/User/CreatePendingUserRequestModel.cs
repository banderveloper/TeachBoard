﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using TeachBoard.IdentityService.Application.Converters;
using TeachBoard.IdentityService.Application.Features.Commands;
using TeachBoard.IdentityService.Application.Mappings;
using TeachBoard.IdentityService.Domain.Enums;

namespace TeachBoard.IdentityService.WebApi.Models.User;

public class CreatePendingUserRequestModel : IMappable
{
    [JsonConverter(typeof(JsonStringEnumConverter<UserRole>))]
    public UserRole Role { get; set; } = UserRole.Unspecified;

    [Required]
    [MinLength(3, ErrorMessage = "First name too short")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MinLength(3, ErrorMessage = "Last name too short")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MinLength(3, ErrorMessage = "Patronymic too short")]
    public string Patronymic { get; set; } = string.Empty;

    [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$",
        ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

    public string? HomeAddress { get; set; }

    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
        ErrorMessage = "Invalid email address format")]
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePendingUserRequestModel, CreatePendingUserCommand>();
    }
}