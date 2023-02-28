using Refit;
using TeachBoard.Gateway.Application.Models.Members;
using TeachBoard.Gateway.Application.Models.Members.Request;
using TeachBoard.Gateway.Application.Models.Members.Response;

namespace TeachBoard.Gateway.Application.RefitClients;

public interface IMembersClient
{
    /// <summary>
    /// Create student, passing user id and group id to members
    /// </summary>
    /// <param name="model">User id and group id</param>
    /// <returns>Created student</returns>
    [Post("/students/create")]
    Task<Student> CreateStudent(CreateStudentRequestModel model);

    /// <summary>
    /// Get group members of student with given user id
    /// </summary>
    /// <param name="userId">Student user id</param>
    /// <returns>Model with list of students</returns>
    [Get("/students/getGroupMembersByUserId/{userId}")]
    Task<StudentsListModel> GetStudentGroupMembersByUserId(int userId);

    /// <summary>
    /// Get student group by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>Student group</returns>
    [Get("/groups/getByUserId/{userId}")]
    Task<Group> GetStudentGroupByUserId(int userId);

    /// <summary>
    /// Get student by user id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Student with given user id</returns>
    [Get("/students/getByUserId/{userId}")]
    Task<Student> GetStudentByUserId(int userId);

    /// <summary>
    /// Get teachers by ids
    /// </summary>
    /// <param name="ids">Ids of teachers</param>
    /// <returns>List of teachers</returns>
    [Get("/teachers/getByIds")]
    Task<TeachersListModel> GetTeachersByIds([Body] List<int> ids);
}