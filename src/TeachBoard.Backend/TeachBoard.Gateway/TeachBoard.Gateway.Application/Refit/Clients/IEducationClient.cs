using Refit;
using TeachBoard.Gateway.Application.Refit.RequestModels.Education;
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

    /// <summary>
    /// Get student's lessons activities as presentation model
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of student's lessons activities as presentation model</returns>
    [Get("/lessons/student-activities/{studentId}")]
    Task<ServiceTypedResponse<IList<StudentLessonActivityPresentationDataModel>>> GetLessonsActivity(int studentId);

    /// <summary>
    /// Complete homework (create completed homework from existing homework)
    /// </summary>
    /// <param name="model">Completed homework data</param>
    /// <returns>Completed homework</returns>
    [Post("/homeworks/complete")]
    Task<ServiceTypedResponse<CompletedHomework>> CompleteHomework([Body] CompleteHomeworkInternalRequestModel model);

    /// <summary>
    /// Get all lessons for given group
    /// </summary>
    /// <param name="groupId">Group id</param>
    /// <returns>List of lessons to given group as presentation data</returns>
    [Get("/lessons/by-group/{groupId}")]
    Task<ServiceTypedResponse<IList<LessonPresentationDataModel>>> GetGroupLessons(int groupId);

    /// <summary>
    /// Create lesson as administrator
    /// </summary>
    /// <param name="model">Group id, subject id, teacher id, scheduled time</param>
    /// <returns>Created lesson</returns>
    [Post("/lessons")]
    Task<ServiceTypedResponse<Lesson>>
        CreateLessonAsAdministrator([Body] CreateLessonAsAdministratorRequestModel model);

    /// <summary>
    /// Get list of teacher's unchecked homeworks count
    /// </summary>
    /// <returns>List of models (teacherId - uncheckedCount)</returns>
    [Get("/homeworks/teachers-unchecked-count")]
    Task<ServiceTypedResponse<IList<TeacherUncheckedHomeworksCountModel>>> GetTeachersUncheckedHomeworksCount();

    /// <summary>
    /// Check completed homework as teacher
    /// </summary>
    /// <param name="model">Checking teacher id, grade and comment</param>
    /// <returns>Completed homework with set mark</returns>
    [Post("/homeworks/check-completed")]
    Task<ServiceTypedResponse<CompletedHomework>> CheckHomework([Body] CheckHomeworkInternalRequestModel model);

    /// <summary>
    /// Get teacher's unchecked homeworks
    /// </summary>
    /// <param name="teacherId">Teacher id</param>
    /// <returns>List of completed unchecked homeworks</returns>
    [Get("/homeworks/teacher-unchecked-homeworks/{teacherId}")]
    Task<ServiceTypedResponse<IList<CompletedHomework>>> GetTeacherUncheckedHomeworks(int teacherId);

    /// <summary>
    /// Get future lessons by teacher id
    /// </summary>
    /// <param name="teacherId">Teacher id</param>
    /// <returns>List of lessons</returns>
    [Get("/lessons/future/{teacherId}")]
    Task<ServiceTypedResponse<IList<Lesson>>> GetFutureLessonsByTeacherId(int teacherId);

    /// <summary>
    /// Get lesson by id
    /// </summary>
    /// <param name="lessonId">Lesson id</param>
    /// <returns>Lesson</returns>
    [Get("/lessons/{lessonId}")]
    Task<ServiceTypedResponse<Lesson?>> GetLessonById(int lessonId);

    /// <summary>
    /// Get lesson's students activities
    /// </summary>
    /// <param name="lessonId">Lesson id</param>
    /// <param name="studentId">List of students ids/param>
    /// <returns>List of student activities</returns>
    [Get("/lessons/students-lesson-activities/{lessonId}")]
    Task<ServiceTypedResponse<IList<StudentLessonActivity>>> GetLessonStudentsActivities(int lessonId,
        [Query(CollectionFormat.Multi)] List<int> studentId);
}
