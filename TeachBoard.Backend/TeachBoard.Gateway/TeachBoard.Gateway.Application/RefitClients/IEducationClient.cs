﻿using Refit;
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
    [Get("/lessons/getByGroupId/{groupId}")]
    Task<LessonsListModel> GetLessonsByGroupId(int groupId);

    /// <summary>
    /// Get student examinations public data
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of student examinations public data</returns>
    [Get("/examinations/getStudentActivities/{studentId}")]
    Task<StudentExaminationsPublicDataListModel> GetStudentExaminationsPublicData(int studentId);

    /// <summary>
    /// Get full information about student's completed homeworks
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of completed homeworks full data</returns>
    [Get("/homeworks/getFullCompletedByStudentId/{studentId}")]
    Task<FullCompletedHomeworksListModel> GetStudentFullCompletedHomeworks(int studentId);

    /// <summary>
    /// Get student's uncompleted homeworks
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Get("/homeworks/getUncompletedHomeworksByStudent")]
    Task<UncompletedHomeworksPublicListModel> GetStudentUncompletedHomeworks(
        [Body] GetUncompletedHomeworksByStudentRequestModel model);

    /// <summary>
    /// Get student's lessons activities
    /// </summary>
    /// <param name="studentId">Student id</param>
    /// <returns>List of all student's lessons activities</returns>
    [Get("/lessons/getStudentActivities/{studentId}")]
    Task<StudentLessonActivityPublicListModel> GetStudentLessonActivities(int studentId);
}