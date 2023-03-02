using Refit;
using TeachBoard.Gateway.Application.Models.Education.Request;
using TeachBoard.Gateway.Application.Models.Education.Response;

namespace TeachBoard.Gateway.Application.RefitClients;

public interface IEducationClient
{
    /// <summary>
    /// Get lessons by group id
    /// </summary>
    /// <param name="groupId">Group id</param>
    /// <returns>Model with list of lessons to given group</returns>
    [Get("/lessons/by-group/{groupId}")]
    Task<LessonsListModel> GetLessonsByGroupId(int groupId);

    /// <summary>
    /// Get student examinations public data
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of student examinations public data</returns>
    [Get("/examinations/student-activities/{studentId}")]
    Task<StudentExaminationsPublicDataListModel> GetStudentExaminationsPublicData(int studentId);

    /// <summary>
    /// Get full information about student's completed homeworks
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of completed homeworks full data</returns>
    [Get("/homeworks/full-completed/{studentId}")]
    Task<FullCompletedHomeworksListModel> GetStudentFullCompletedHomeworks(int studentId);

    /// <summary>
    /// Get student's uncompleted homeworks
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <param name="groupId">Student's group id</param>
    /// <returns>Model with list of uncompleted homeworks at public data</returns>
    [Get("/homeworks/uncompleted-homeworks/{studentId}/{groupId}")]
    Task<UncompletedHomeworksPublicListModel> GetStudentUncompletedHomeworks(int studentId, int? groupId);

    /// <summary>
    /// Get student's lessons activities
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of all student's lessons activities</returns>
    [Get("/lessons/student-activities/{studentId}")]
    Task<StudentLessonActivityPublicListModel> GetStudentLessonActivities(int studentId);
}