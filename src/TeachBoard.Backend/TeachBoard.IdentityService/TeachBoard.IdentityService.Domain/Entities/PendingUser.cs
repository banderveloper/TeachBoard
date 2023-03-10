using TeachBoard.IdentityService.Domain.Enums;

namespace TeachBoard.IdentityService.Domain.Entities;

// 'User' created by administrator, waiting for approval from student
public class PendingUser : BaseEntity
{
    public string RegisterCode { get; set; } = string.Empty; // Code which student must be enter during registration
    public DateTime ExpiresAt { get; set; } // DateTime when pending user will be deleted
    
    public UserRole Role { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
}