using AutoMapper;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Application.Mappings;

namespace TeachBoard.MembersService.WebApi.Models.Student;

public class SetStudentGroupRequestModel : IMappable
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<SetStudentGroupRequestModel, SetStudentGroupCommand>();
    }
}