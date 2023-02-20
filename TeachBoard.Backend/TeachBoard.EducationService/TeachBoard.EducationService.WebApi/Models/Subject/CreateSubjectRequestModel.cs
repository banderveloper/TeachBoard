using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.EducationService.Application.Features.Subject;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Subject;

public class CreateSubjectRequestModel : IMappable
{
    [Required] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required] public int LessonsCount { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateSubjectRequestModel, CreateSubjectCommand>();
    }
}