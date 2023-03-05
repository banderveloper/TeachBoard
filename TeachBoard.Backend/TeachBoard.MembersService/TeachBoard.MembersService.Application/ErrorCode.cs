using System.Text.Json.Serialization;
using TeachBoard.MembersService.Application.Converters;

namespace TeachBoard.MembersService.Application;

/// <summary>
/// Exception error code, sent to client with error model as snake_case_string
/// </summary>
/// <example>StudentAlreadyExists => student_already_exists</example>
[JsonConverter(typeof(SnakeCaseStringEnumConverter<ErrorCode>))]
public enum ErrorCode
{
    Unknown,
    
    StudentNotFound,
    StudentAlreadyExists,
    
    TeacherNotFound,
    TeacherAlreadyExists,
    
    GroupAlreadyExists,
    GroupNotFound
}