using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.MembersService.Application.Features.Feedbacks;
using TeachBoard.MembersService.Application.Mappings;

namespace TeachBoard.MembersService.WebApi.Models.Feedback;

public class CreateStudentToTeacherRequestModel : IMappable
{
    [Required] [Range(1, int.MaxValue)] public int TeacherId { get; set; }
    [Required] [Range(1, int.MaxValue)] public int StudentId { get; set; }
    [Required] [MinLength(6)] [MaxLength(256)] public string Text { get; set; } = string.Empty;
    [Required] [Range(1, 10)] public int Rating { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateStudentToTeacherRequestModel, CreateStudentToTeacherRequestModel>();
    }
}