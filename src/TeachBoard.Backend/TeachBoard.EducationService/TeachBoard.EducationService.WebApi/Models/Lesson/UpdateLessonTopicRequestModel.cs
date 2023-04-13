using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.EducationService.Application.Features.Lesson;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Lesson;

public class UpdateLessonTopicRequestModel : IMappable
{
    [Required] public int LessonId { get; set; }
    [Required] public string Topic { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateLessonTopicRequestModel, UpdateLessonTopicCommand>();
    }
}