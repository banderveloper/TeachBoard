using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

namespace TeachBoard.Gateway.WebApi.Models;

public class CurrentLessonResponseModel
{
    public LessonPresentationDataModel Lesson { get; set; }
    public IEnumerable<StudentPresentationWithActivityModel> Students { get; set; }
}