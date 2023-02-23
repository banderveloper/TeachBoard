using TeachBoard.Gateway.Domain.Enums;

namespace TeachBoard.Gateway.Application.Models.Identity.Response;

public class UserPublicDataModel
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
}