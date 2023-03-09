using AutoMapper;
using TeachBoard.EducationService.Application.Features.Homework;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Homework;

public class GetUncompletedHomeworksByStudentRequestModel : IMappable
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetUncompletedHomeworksByStudentRequestModel, GetUncompletedHomeworksPresentationDataByStudentQuery>();
    }
}