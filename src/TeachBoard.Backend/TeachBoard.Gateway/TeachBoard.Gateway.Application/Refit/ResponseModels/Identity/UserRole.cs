using System.Text.Json.Serialization;
using TeachBoard.Gateway.Application.Converters;

namespace TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;

[JsonConverter(typeof(JsonStringEnumConverter<UserRole>))]
public enum UserRole
{
    Unspecified,
    Student,
    Teacher,
    Administrator,
    Director
}