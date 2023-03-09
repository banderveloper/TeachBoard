using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;

namespace TeachBoard.Gateway.WebApi.Models;


public class MemberFullDataResponseModel
{
    public UserPublicData User { get; set; }
    
    // special member info (about student/teacher/...)
    public object? Member { get; set; }
}