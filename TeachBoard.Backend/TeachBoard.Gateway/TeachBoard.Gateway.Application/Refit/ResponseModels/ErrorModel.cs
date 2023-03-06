namespace TeachBoard.Gateway.Application.Refit.ResponseModels;

public class ErrorModel
{
    public ErrorCode ErrorCode { get; set; }
    public string? ReasonField { get; set; }
    public string? Message { get; set; }
}