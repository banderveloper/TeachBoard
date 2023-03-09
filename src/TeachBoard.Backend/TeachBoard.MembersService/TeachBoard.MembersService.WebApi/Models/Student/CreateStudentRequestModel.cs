using System.ComponentModel.DataAnnotations;

namespace TeachBoard.MembersService.WebApi.Models.Student;

public class CreateStudentRequestModel
{
    [Required] [Range(1, int.MaxValue)] public int UserId { get; set; }
    [Range(1, int.MaxValue)] public int? GroupId { get; set; }
}