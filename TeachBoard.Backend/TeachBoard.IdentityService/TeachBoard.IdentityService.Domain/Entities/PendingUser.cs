namespace TeachBoard.IdentityService.Domain.Entities;

// 'User' created by administrator, waiting for approval from student
public class PendingUser : BaseEntity
{
    // Code which student must be enter during registration
    public string RegisterCode { get; set; } = string.Empty;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }

    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    public DateTime ExpiresAt { get; set; }
}