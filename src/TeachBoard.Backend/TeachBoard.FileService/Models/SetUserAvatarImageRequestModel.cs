using System.ComponentModel.DataAnnotations;

namespace TeachBoard.FileService.Models;

public class SetUserAvatarRequestModel
{
    [Required] public int UserId { get; set; }
    [Required] public IFormFile ImageFile { get; set; }
}