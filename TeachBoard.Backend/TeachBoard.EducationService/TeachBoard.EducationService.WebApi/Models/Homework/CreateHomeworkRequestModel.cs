using AutoMapper;
using TeachBoard.EducationService.Application.Features.Homework;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Homework;

public class CreateHomeworkRequestModel : IMappable
{
    public int GroupId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public string? FilePath { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateHomeworkRequestModel, CreateHomeworkCommand>();
    }
}