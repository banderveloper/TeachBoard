namespace TeachBoard.IdentityService.Application.Exceptions;

public interface IApiException
{
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
}