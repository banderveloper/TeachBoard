using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Teachers.Common;

public class TeachersListModel
{
    public IList<Teacher> Teachers { get; set; }
}