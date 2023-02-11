namespace TeachBoard.Application.Exceptions;

public class NotFoundException : Exception, IApiException
{
    public string? Error { get; set; } 
    public string? ErrorDescription { get; set; } 
}