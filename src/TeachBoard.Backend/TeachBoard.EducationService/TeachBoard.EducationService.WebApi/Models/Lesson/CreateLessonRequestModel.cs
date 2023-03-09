using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.EducationService.Application.Features.Lesson;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Lesson;

public class CreateLessonRequestModel : IMappable
{
    [Required] public int SubjectId { get; set; }
    [Required] public int TeacherId { get; set; }
    [Required] public int GroupId { get; set; }
    public string? Topic { get; set; }
    public string? Classroom { get; set; }

    [Required] public DateTime StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateLessonRequestModel, CreateLessonCommand>();
    }
}