using System.Text.Json.Serialization;

namespace TeachBoard.IdentityService.Application.Exceptions;

public interface INotAcceptableRequestException
{
    /// <summary>
    /// String respesentation of error code
    /// </summary>
    /// <example>refresh_token_not_passed</example>
    [JsonPropertyName("error")]
    public string ErrorCode { get; set; }
}

public class NotAcceptableRequestException : Exception, INotAcceptableRequestException
{
    public string ErrorCode { get; set; } = "unknown_error";
}