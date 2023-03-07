using Refit;
using TeachBoard.Gateway.Application.Refit.RequestModels.Members;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Members;

namespace TeachBoard.Gateway.Application.Refit.Clients;

public interface IMembersClient
{
    /// <summary>
    /// Create student, passing user id and group id to members
    /// </summary>
    /// <param name="model">User id and group id</param>
    /// <returns>Created student</returns>
    [Post("/students")]
    Task<ServiceTypedResponse<Student>> CreateStudent(CreateStudentRequestModel model);
    
    /// <summary>
    /// Get group members of student with given user id
    /// </summary>
    /// <param name="userId">Student user id</param>
    /// <returns>List of students</returns>
    [Get("/students/group-members-by-user/{userId}")]
    Task<ServiceTypedResponse<IList<Student>>> GetStudentGroupMembersByUserId(int userId);
    
    /// <summary>
    /// Get student group by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>Student group</returns>
    [Get("/groups/by-user/{userId}")]
    Task<ServiceTypedResponse<Group>> GetStudentGroupByUserId(int userId);

    /// <summary>
    /// Get student by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>Student</returns>
    [Get("/students/by-user/{userId}")]
    Task<ServiceTypedResponse<Student>> GetStudentByUserId(int userId);
}