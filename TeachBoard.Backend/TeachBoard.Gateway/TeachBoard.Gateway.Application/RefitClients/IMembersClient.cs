using Refit;
using TeachBoard.Gateway.Application.Models.Members;

namespace TeachBoard.Gateway.Application.RefitClients;

public interface IMembersClient
{
    [Post("/student/create")]
    Task<StudentCreateResponseModel> CreateStudent(StudentCreateRequestModel model);
}