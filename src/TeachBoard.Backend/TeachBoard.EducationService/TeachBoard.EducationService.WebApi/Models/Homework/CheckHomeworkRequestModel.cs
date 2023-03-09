using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.EducationService.Application.Features.Homework;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Homework;

public class CheckHomeworkRequestModel : IMappable
{
    [Required] public int TeacherId { get; set; }
    [Required] public int CompletedHomeworkId { get; set; }
    [Required] [Range(1, 12)] public int Grade { get; set; }
    public string? Comment { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CheckHomeworkRequestModel, CheckHomeworkCommand>();
    }
}