namespace TeachBoard.IdentityService.Application.Exceptions;

public class DataExpiredException : Exception, IApiException
{
    public string? Error { get; set; } 
    public string? ErrorDescription { get; set; } 
}