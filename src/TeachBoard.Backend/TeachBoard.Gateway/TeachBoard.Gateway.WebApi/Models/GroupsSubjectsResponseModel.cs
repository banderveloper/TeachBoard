using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Members;

namespace TeachBoard.Gateway.WebApi.Models;

public class GroupsSubjectsResponseModel
{
    public IList<Group> Groups { get; set; }
    public IList<Subject> Subjects { get; set; }
}