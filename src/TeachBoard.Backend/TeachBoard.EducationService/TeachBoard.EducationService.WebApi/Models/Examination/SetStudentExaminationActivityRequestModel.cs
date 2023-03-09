using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using TeachBoard.EducationService.Application.Converters;
using TeachBoard.EducationService.Application.Features.Examination;
using TeachBoard.EducationService.Application.Mappings;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.WebApi.Models.Examination;

public class SetStudentExaminationActivityRequestModel : IMappable
{
    public int? Grade { get; set; }
    
    [Required] public int StudentId { get; set; }
    [Required] public int ExaminationId { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter<StudentExaminationStatus>))]
    public StudentExaminationStatus Status { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SetStudentExaminationActivityRequestModel, SetStudentExaminationActivityCommand>();
    }
}