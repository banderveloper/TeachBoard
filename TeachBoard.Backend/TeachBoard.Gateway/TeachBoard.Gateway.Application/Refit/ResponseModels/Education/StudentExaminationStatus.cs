using System.Text.Json.Serialization;
using TeachBoard.Gateway.Application.Converters;

namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

[JsonConverter(typeof(JsonStringEnumConverter<StudentExaminationStatus>))]
public enum StudentExaminationStatus
{
    Passed = 0,
    Banned = 1,
    Absent = 2
}