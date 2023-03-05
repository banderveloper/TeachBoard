using System.Text.Json.Serialization;

namespace TeachBoard.IdentityService.Application.Exceptions;

public interface IExpectedApiException
{
    /// <summary>
    /// String respesentation of error code
    /// </summary>
    /// <example>username_already_exists</example>
    public string ErrorCode { get; set; }

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
    public string ErrorCode { get; set; } = "unknown_error";
    public string? ReasonField { get; set; }
    public string? PublicErrorMessage { get; set; }
    public string? LogErrorMessage { get; set; }
}