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
    Task<ServiceTypedResponse<IList<StudentExaminationActivityPresentationDataModel>>> GetStudentExaminationsActivities(int studentId);
}