using System.ComponentModel.DataAnnotations;

namespace TeachBoard.MembersService.WebApi.Models.Group;

public class CreateGroupRequestModel
{
    [Required]
    [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{1,15}$",
        ErrorMessage = "Group name is invalid. Use only latin letters, numbers and _, length 2-16 symbols")]
    public string Name { get; set; } = string.Empty;
}