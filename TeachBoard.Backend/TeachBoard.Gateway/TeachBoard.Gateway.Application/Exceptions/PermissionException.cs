namespace TeachBoard.Gateway.Application.Exceptions;

public class PermissionException : Exception, IApiException
{
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
    public string? ReasonField { get; set; }
}