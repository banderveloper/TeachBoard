using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.EducationService.Application.Features.Homework;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Homework;

public class CreateHomeworkRequestModel : IMappable
{
    [Required] public int GroupId { get; set; }
    [Required] public int SubjectId { get; set; }
    [Required] public int TeacherId { get; set; }
    [Required] public string FilePath { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateHomeworkRequestModel, CreateHomeworkCommand>();
    }
}