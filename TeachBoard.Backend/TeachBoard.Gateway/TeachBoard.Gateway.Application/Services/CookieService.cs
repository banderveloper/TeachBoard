using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace TeachBoard.Gateway.Application.Services;

public class CookieService
{
    public void TransferCookies(HttpResponseHeaders sourceHeaders, IResponseCookies destinationCookies)
    {
        // Retrieve the cookies from the response headers
        var cookies = sourceHeaders.GetValues("Set-Cookie");

        foreach (var cookie in cookies)
        {
            var setCookieHeader = SetCookieHeaderValue.Parse(cookie);

            // Create a new CookieOptions object from the parsed cookie attributes
            var cookieOptions = new CookieOptions
            {
                Expires = setCookieHeader.Expires ?? default(DateTimeOffset),
                Path = setCookieHeader.Path.HasValue ? setCookieHeader.Path.Value : "/",
                SameSite = (SameSiteMode)setCookieHeader.SameSite,
                Secure = setCookieHeader.Secure,
                HttpOnly = setCookieHeader.HttpOnly
            };

            // Add the cookie to the response
            destinationCookies.Append(setCookieHeader.Name.ToString(), setCookieHeader.Value.ToString(), cookieOptions);
        }
    }
}