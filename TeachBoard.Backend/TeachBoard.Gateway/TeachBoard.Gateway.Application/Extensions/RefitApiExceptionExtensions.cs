using TeachBoard.Gateway.Application.Exceptions;

namespace TeachBoard.Gateway.Application.Extensions;

public static class RefitApiExceptionExtensions
{
    public static async Task<ServiceException> ToServiceException(this Refit.ApiException refitException)
    {
        var responseBody = await refitException.GetContentAsAsync<Dictionary<string, object>>();
        return new ServiceException
        {
            Error = responseBody?.GetValueOrDefault("error")?.ToString(),
            ErrorDescription = responseBody?.GetValueOrDefault("errorDescription")?.ToString(),
            ReasonField = responseBody?.GetValueOrDefault("reasonField")?.ToString(),
        };
    }

    public static async Task<Dictionary<string, object>?> ToValidationResultDictionary(
        this Refit.ApiException refitException)
    {
        return await refitException.GetContentAsAsync<Dictionary<string, object>>();
    }
}