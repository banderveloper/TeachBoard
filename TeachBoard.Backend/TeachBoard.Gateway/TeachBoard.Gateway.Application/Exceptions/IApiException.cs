namespace TeachBoard.Gateway.Application.Exceptions;

public interface IApiException
{
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
    public string? ReasonField { get; set; }

    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { "error", Error },
            { "errorDescription", ErrorDescription },
            { "reasonField", ReasonField }
        };
    }
}