using System.Text.Json.Serialization;

namespace TeachBoard.FileService.Application.Exceptions;

public interface IHostingErrorException
{
    /// <summary>
    /// Error code, added to response error model as snake_case_string
    /// </summary>
    public ErrorCode ErrorCode { get; set; }

    /// <summary>
    /// Public error message, sent to client
    /// </summary>
    /// <example>User with given username already exists</example>
    [JsonPropertyName("message")]
    public string? PublicErrorMessage { get; set; }
}

public class HostingErrorException : Exception, IHostingErrorException
{
    public ErrorCode ErrorCode { get; set; }
    public string? PublicErrorMessage { get; set; }
}