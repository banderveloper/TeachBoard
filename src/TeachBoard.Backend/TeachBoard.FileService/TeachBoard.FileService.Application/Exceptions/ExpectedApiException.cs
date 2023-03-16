using System.Text.Json.Serialization;

namespace TeachBoard.FileService.Application.Exceptions;

public interface IExpectedApiException
{
    /// <summary>
    /// Error code, added to response error model as snake_case_string
    /// </summary>
    public ErrorCode ErrorCode { get; set; }

    /// <summary>
    /// Model field/property, caused an error
    /// </summary>
    /// <example>username</example>
    public string? ReasonField { get; set; }

    /// <summary>
    /// Public error message, sent to client
    /// </summary>
    /// <example>User with given username already exists</example>
    [JsonPropertyName("message")]
    public string? PublicErrorMessage { get; set; }

    /// <summary>
    /// Private error message for logger
    /// </summary>
    /// <example>User with username 'banderveloper' already exists</example>
    [JsonIgnore]
    public string? LogErrorMessage { get; set; }
}

public class ExpectedApiException : Exception, IExpectedApiException
{
    public ErrorCode ErrorCode { get; set; } = ErrorCode.Unknown;
    public string? ReasonField { get; set; }
    public string? PublicErrorMessage { get; set; }
    public string? LogErrorMessage { get; set; }
}