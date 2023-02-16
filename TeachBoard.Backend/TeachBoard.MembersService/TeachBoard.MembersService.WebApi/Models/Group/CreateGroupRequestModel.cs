using System.ComponentModel.DataAnnotations;

namespace TeachBoard.MembersService.WebApi.Models.Group;

public class CreateGroupRequestModel
{
    [Required] public string Name { get; set; } = string.Empty;
}