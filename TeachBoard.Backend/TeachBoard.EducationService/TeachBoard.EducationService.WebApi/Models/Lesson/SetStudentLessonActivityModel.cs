using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TeachBoard.EducationService.Application.Features.Lesson;
using TeachBoard.EducationService.Application.Mappings;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.WebApi.Models.Lesson;

public class SetStudentLessonActivityModel : IMappable
{
    [Required] public int StudentId { get; set; }
    [Required] public int LessonId { get; set; }

    [Required]
    [EnumDataType(typeof(AttendanceStatus), ErrorMessage = "Invalid attendance status, 0-2 expected")]
    public AttendanceStatus AttendanceStatus { get; set; }

    public int? Grade { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SetStudentLessonActivityModel, SetStudentLessonActivityCommand>();
    }
}