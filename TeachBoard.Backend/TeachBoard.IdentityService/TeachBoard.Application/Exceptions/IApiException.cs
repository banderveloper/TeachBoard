namespace TeachBoard.Application.Exceptions;

public interface IApiException
{
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
}