using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.EducationService.Application.Features.Homework;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Homework;

public class CompleteHomeworkRequestModel : IMappable
{
    [Required] public int HomeworkId { get; set; }
    [Required] public int StudentId { get; set; }
    public string? FilePath { get; set; }
    public string? StudentComment { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CompleteHomeworkRequestModel, CompleteHomeworkCommand>();
    }
}