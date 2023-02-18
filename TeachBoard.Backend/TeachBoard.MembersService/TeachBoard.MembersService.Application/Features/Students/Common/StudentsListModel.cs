using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Students.Common;

public class StudentsListModel
{
    public IList<Student> Students { get; set; }
}