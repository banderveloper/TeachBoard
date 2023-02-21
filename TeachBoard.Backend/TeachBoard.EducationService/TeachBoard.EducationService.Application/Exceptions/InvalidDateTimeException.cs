namespace TeachBoard.EducationService.Application.Exceptions;

public class InvalidDateTimeException : Exception, IApiException
{
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
    public string? ReasonField { get; set; }
}