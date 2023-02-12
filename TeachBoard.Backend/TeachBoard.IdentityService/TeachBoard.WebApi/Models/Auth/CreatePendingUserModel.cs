﻿using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.Application.CQRS.Commands.CreatePendingUser;
using TeachBoard.Application.Mappings;

namespace TeachBoard.WebApi.Models.Auth;

public class CreatePendingUserModel : IMappable
{
    [MinLength(3, ErrorMessage = "First name too short")]
    public string? FirstName { get; set; }

    [MinLength(3, ErrorMessage = "Last name too short")]
    public string? LastName { get; set; }

    [MinLength(3, ErrorMessage = "Patronymic too short")]
    public string? Patronymic { get; set; }

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
        profile.CreateMap<CreatePendingUserModel, CreatePendingUserCommand>();
    }
}