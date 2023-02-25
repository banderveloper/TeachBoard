using AutoMapper;
using TeachBoard.EducationService.Application.Features.Homework;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Homework;

public class CheckHomeworkRequestModel : IMappable
{
    public int TeacherId { get; set; }
    public int CompletedHomeworkId { get; set; }
    public int Grade { get; set; }
    public string? Comment { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CheckHomeworkRequestModel, CheckHomeworkCommand>();
    }
}