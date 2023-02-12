namespace TeachBoard.IdentityService.Application.Exceptions;

public class RefreshTokenException : Exception, IApiException
{
    public string? Error { get; set; } 
    public string? ErrorDescription { get; set; } 
}