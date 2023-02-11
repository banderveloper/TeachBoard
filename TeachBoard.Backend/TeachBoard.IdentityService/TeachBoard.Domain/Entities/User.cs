using System.Text.Json.Serialization;

namespace TeachBoard.Domain.Entities;

// User of teachboard (anyone - student, teacher, admin)
public class User : BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
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
    
    //
    [JsonIgnore] public IList<Role>? Roles { get; set; }
}