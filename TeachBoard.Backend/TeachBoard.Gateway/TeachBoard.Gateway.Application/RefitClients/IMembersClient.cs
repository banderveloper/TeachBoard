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
    [Post("/students")]
    Task<Student> CreateStudent(CreateStudentRequestModel model);

    /// <summary>
    /// Get group members of student with given user id
    /// </summary>
    /// <param name="userId">Student user id</param>
    /// <returns>Model with list of students</returns>
    [Get("/students/group-members-by-user/{userId}")]
    Task<StudentsListModel> GetStudentGroupMembersByUserId(int userId);

    /// <summary>
    /// Get student group by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>Student group</returns>
    [Get("/groups/by-user/{userId}")]
    Task<Group> GetStudentGroupByUserId(int userId);

    /// <summary>
    /// Get student by user id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Student with given user id</returns>
    [Get("/students/by-user/{userId}")]
    Task<Student> GetStudentByUserId(int userId);

    /// <summary>
    /// Get teachers by ids
    /// </summary>
    /// <param name="teacherId">Ids of teachers</param>
    /// <returns>List of teachers</returns>
    [Get("/teachers/by-ids")]
    Task<TeachersListModel> GetTeachersByIds([Query(CollectionFormat.Multi)] List<int> teacherId);

    /// <summary>
    /// Set student group
    /// </summary>
    /// <param name="model">Model with student id and group id</param>
    /// <returns>Default refit response</returns>
    [Put("/students/student-group")]
    Task<IApiResponse> SetStudentGroup([Body] SetStudentGroupRequestModel model);
}