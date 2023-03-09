using System.Text.Json.Serialization;
using TeachBoard.Gateway.Application.Converters;

namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Education;

[JsonConverter(typeof(JsonStringEnumConverter<AttendanceStatus>))]
public enum AttendanceStatus
{
    Attended,
    Late,
    Absent
}