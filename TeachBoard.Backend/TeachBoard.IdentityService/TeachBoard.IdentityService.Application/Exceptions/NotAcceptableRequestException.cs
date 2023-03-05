namespace TeachBoard.IdentityService.Application.Exceptions;

public interface INotAcceptableRequestException
{
    /// <summary>
    /// Error code, added to response error model as snake_case_string
    /// </summary>
    public ErrorCode ErrorCode { get; set; }
}

public class NotAcceptableRequestException : Exception, INotAcceptableRequestException
{
    public ErrorCode ErrorCode { get; set; } = ErrorCode.Unknown;
}