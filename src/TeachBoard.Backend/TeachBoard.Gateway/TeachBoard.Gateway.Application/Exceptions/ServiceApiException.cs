using System.Net;

namespace TeachBoard.Gateway.Application.Exceptions;

public interface IServiceApiException
{
    public object? Error { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public class ServiceApiException : Exception, IServiceApiException
{
    public object? Error { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}