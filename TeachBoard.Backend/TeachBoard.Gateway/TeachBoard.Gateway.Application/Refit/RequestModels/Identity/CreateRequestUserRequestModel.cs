using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;

namespace TeachBoard.Gateway.Application.Refit.RequestModels.Identity;

public class CreatePendingUserRequestModel
{
    public UserRole Role { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }

    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }
}