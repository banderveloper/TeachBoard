namespace TeachBoard.Application.Exceptions;

public class AlreadyExistsException : Exception, IApiException
{
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
}