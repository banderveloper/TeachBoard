namespace TeachBoard.Gateway.Application.Refit;

public class ServiceTypedResponse<T>
{
    public T? Data { get; set; }
    public object? Error { get; set; }
}