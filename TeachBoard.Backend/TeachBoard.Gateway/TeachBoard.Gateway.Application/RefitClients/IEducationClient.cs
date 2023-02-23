using Refit;
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
}