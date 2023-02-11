﻿using MediatR;

namespace TeachBoard.Application.CQRS.Commands.CreateUser;

public class CreateUserCommand : IRequest<CreatedUserModel>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    
    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
}