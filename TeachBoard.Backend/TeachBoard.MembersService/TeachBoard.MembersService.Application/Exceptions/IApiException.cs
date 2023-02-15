namespace TeachBoard.MembersService.Application.Exceptions;

/// <summary>
/// Parent of all handled exceptions for response error model
/// </summary>
public interface IApiException
{
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
}