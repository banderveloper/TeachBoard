using System.ComponentModel.DataAnnotations;

namespace TeachBoard.MembersService.WebApi.Models.Teacher;

public class CreateTeacherRequestModel
{
    [Required] [Range(1, int.MaxValue)] public int UserId { get; set; }
}