using System.ComponentModel.DataAnnotations;

namespace TeachBoard.Gateway.Application.Refit.RequestModels.Identity;

public class UpdateUserRequestModel
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }

    public string? PhoneNumber { get; set; }
    public string? HomeAddress { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
}