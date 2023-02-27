using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.EducationService.Application.Features.Examination;
using TeachBoard.EducationService.Application.Mappings;

namespace TeachBoard.EducationService.WebApi.Models.Examination;

public class CreateExaminationRequestModel : IMappable
{
    [Required] public int SubjectId { get; set; }
    [Required] public int GroupId { get; set; }
    [Required] public DateTime StartsAt { get; set; }
    [Required] public DateTime EndsAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateExaminationRequestModel, CreateExaminationCommand>();
    }
}