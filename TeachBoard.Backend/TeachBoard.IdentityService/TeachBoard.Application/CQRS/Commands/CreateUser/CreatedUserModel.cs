﻿namespace TeachBoard.Application.CQRS.Commands.CreateUser;

public class CreatedUserModel
{
    public string UserName { get; set; } = string.Empty;
    public string PasswordEncrypted { get; set; } = string.Empty;
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    
    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
}