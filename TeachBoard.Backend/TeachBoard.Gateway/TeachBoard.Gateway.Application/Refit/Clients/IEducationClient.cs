using Refit;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

namespace TeachBoard.Gateway.Application.Refit.Clients;

public interface IEducationClient
{
    /// <summary>
    /// Get student's examinations activities as presentation model
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of student's examinations activities as presentation model</returns>
    [Get("/examinations/student-activities/{studentId}")]
    Task<ServiceTypedResponse<IList<StudentExaminationActivityPresentationDataModel>>> GetExaminationsActivities(int studentId);

    /// <summary>
    /// Get student's completed homeworks as presentation model
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of student's completed homeworks as presentation model</returns>
    [Get("/homeworks/completed/{studentId}")]
    Task<ServiceTypedResponse<IList<CompletedHomeworkPresentationDataModel>>> GetCompletedHomeworks(int studentId);

    /// <summary>
    /// Get student's uncompleted homeworks as presentation model
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <param name="groupId">Student's group id</param>
    /// <returns>List of student's uncompleted homeworks as presentation model</returns>
    [Get("/homeworks/uncompleted-homeworks/{studentId}/{groupId}")]
    Task<ServiceTypedResponse<IList<UncompletedHomeworkPresentationDataModel>>> GetUncompletedHomeworks(int studentId,
        int? groupId);
}