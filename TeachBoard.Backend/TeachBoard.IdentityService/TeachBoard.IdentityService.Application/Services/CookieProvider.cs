using Microsoft.AspNetCore.Http;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.Exceptions;

namespace TeachBoard.IdentityService.Application.Services;

public class CookieProvider
{
    private readonly CookieConfiguration _cookieConfiguration;
    
    public CookieProvider(CookieConfiguration cookieConfiguration) =>
        _cookieConfiguration = cookieConfiguration;
    
    // Insert refresh token into response http-only cookie
    public void AddRefreshCookieToResponse(HttpResponse response, Guid refreshToken)
    {
        response.Cookies.Append(_cookieConfiguration.RefreshCookieName, refreshToken.ToString(),
            new CookieOptions
            {
                HttpOnly = true, 
                SameSite = SameSiteMode.Lax, 
                Expires = new DateTimeOffset(DateTime.Now.AddHours(_cookieConfiguration.RefreshCookieLifetimeHours))
            });
    }

    // Extract refresh token from http-only cookie
    public Guid GetRefreshTokenFromCookie(HttpRequest request)
    {
        if (!request.Cookies.ContainsKey(_cookieConfiguration.RefreshCookieName))
            throw new NotAcceptableRequestException { ErrorCode = ErrorCode.CookieRefreshTokenNotPassed };
        
        // Try to extract refresh token from cookie. If it is absent - exception
        if (!request.Cookies.TryGetValue(_cookieConfiguration.RefreshCookieName, out var refreshToken))
            throw new NotAcceptableRequestException { ErrorCode = ErrorCode.CookieRefreshTokenNotPassed };

        return Guid.Parse(refreshToken);
    }
}