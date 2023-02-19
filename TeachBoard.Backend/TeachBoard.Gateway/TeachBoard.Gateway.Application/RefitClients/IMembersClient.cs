using Refit;
using TeachBoard.Gateway.Application.Models.Members;
using TeachBoard.Gateway.Application.Models.Members.Request;

namespace TeachBoard.Gateway.Application.RefitClients;

public interface IMembersClient
{
    /// <summary>
    /// Create student, passing user id and group id to members
    /// </summary>
    /// <param name="model">User id and group id</param>
    /// <returns>Created student</returns>
    [Post("/student/create")]
    Task<Student> CreateStudent(CreateStudentRequestModel model);
}