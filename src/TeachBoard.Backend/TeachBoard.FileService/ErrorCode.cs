using System.Text.Json.Serialization;
using TeachBoard.FileService.Converters;

namespace TeachBoard.FileService;

/// <summary>
/// Exception error code, sent to client with error model as snake_case_string
/// </summary>
/// <example>CookieRefreshTokenNotPassed => cookie_refresh_token_not_passed</example>
[JsonConverter(typeof(SnakeCaseStringEnumConverter<ErrorCode>))]
public enum ErrorCode
{
    Unknown,
    FileEmpty
}