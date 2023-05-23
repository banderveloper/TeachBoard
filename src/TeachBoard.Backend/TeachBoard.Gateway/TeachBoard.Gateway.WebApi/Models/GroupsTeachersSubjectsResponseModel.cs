using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Members;

namespace TeachBoard.Gateway.WebApi.Models;

public class GroupsTeachersSubjectsResponseModel
{
    public IList<TeacherPresentation> Teachers { get; set; }
    public IList<Group> Groups { get; set; }
    public IList<Subject> Subjects { get; set; }
}

public class TeacherPresentation
{
    public int UserId { get; set; }
    public int TeacherId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string AvatarImagePath { get; set; }
}