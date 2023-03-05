using System.Text.Json.Serialization;

namespace TeachBoard.MembersService.Application.Exceptions;

public interface IExpectedApiException
{
    /// <summary>
    /// String representation of error code
    /// </summary>
    /// <example>username_already_exists</example>
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
    public string? PublicErrorMessage { get; set; }

    /// <summary>
    /// Private error message for logger
    /// </summary>
    /// <example>User with username 'banderveloper' already exists</example>
    public string? LogErrorMessage { get; set; }
}

public class ExpectedApiException : Exception, IExpectedApiException
{
    public ErrorCode ErrorCode { get; set; }
    public string? ReasonField { get; set; }
    public string? PublicErrorMessage { get; set; }
    public string? LogErrorMessage { get; set; }
}
