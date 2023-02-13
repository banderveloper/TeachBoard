namespace TeachBoard.IdentityService.Application.Exceptions;

public class WrongPasswordException : Exception, IApiException
{
    public string? Error { get; set; } 
    public string? ErrorDescription { get; set; } 
}