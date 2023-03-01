using TeachBoard.Gateway.Domain.Enums;

namespace TeachBoard.Gateway.Application.Models.Identity.Request;

public class CreatePendingUserRequestModel
{
    public int Role { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }

    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }
}