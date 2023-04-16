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

    /// <summary>
    /// Set student group
    /// </summary>
    /// <param name="model">Student id and group id</param>
    /// <returns>Empty response</returns>
    [Put("/students/student-group")]
    Task<ServiceTypedResponse<object>> SetStudentGroup([Body] SetStudentGroupRequestModel model);

    /// <summary>
    /// Get teacher by id
    /// </summary>
    /// <param name="teacherId">Teacher id</param>
    /// <returns>Teacher or null</returns>
    [Get("/teachers/{teacherId}")]
    Task<ServiceTypedResponse<Teacher?>> GetTeacherById(int teacherId);

    /// <summary>
    /// Get group by id
    /// </summary>
    /// <param name="groupId">Group id</param>
    /// <returns>Group or null</returns>
    [Get("/groups/{groupId}")]
    Task<ServiceTypedResponse<Group?>> GetGroupById(int groupId);

    /// <summary>
    /// Get student presentation model (with group info) by user id
    /// </summary>
    /// <param name="userId">Student's user id</param>
    /// <returns>Student info with group info</returns>
    [Get("/students/presentation/{userId}")]
    Task<ServiceTypedResponse<StudentPresentationModel?>> GetStudentPresentation(int userId);

    /// <summary>
    /// Get teacher by user id
    /// </summary>
    /// <param name="userId">Teacher's user id</param>
    /// <returns>Teacher or null</returns>
    [Get("/teachers/by-user-id/{userId}")]
    Task<ServiceTypedResponse<Teacher?>> GetTeacherByUserId(int userId);

    /// <summary>
    /// Get teachers by ids
    /// </summary>
    /// <param name="teacherId">List of teachers ids</param>
    /// <returns>List of teachers</returns>
    [Get("/teachers/by-ids")]
    Task<ServiceTypedResponse<IList<Teacher>>> GetTeachersByIds([Query(CollectionFormat.Multi)] List<int> teacherId);

    /// <summary>
    /// Get students by group id
    /// </summary>
    /// <param name="groupId">Group id</param>
    /// <returns>List of students</returns>
    [Get("/students/by-group/{groupId}")]
    Task<ServiceTypedResponse<IList<Student>>> GetStudentsByGroupId(int groupId);

    [Get("/students/{studentId}")]
    Task<ServiceTypedResponse<Student?>> GetStudentById(int studentId);
    
    
    [Get("/groups")]
    Task<ServiceTypedResponse<IList<Group>>> GetAllGroups();
    
    
    [Get("/teachers")]
    Task<ServiceTypedResponse<IList<Teacher>>> GetAllTeachers();
}