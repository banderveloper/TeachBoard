using Microsoft.AspNetCore.Http;
using TeachBoard.IdentityService.Application.Configurations;
using TeachBoard.IdentityService.Application.Exceptions;

namespace TeachBoard.IdentityService.Application.Services;

public class CookieProvider
{
    private readonly CookieConfiguration _cookieConfiguration;
    
    public CookieProvider(CookieConfiguration cookieConfiguration)
    {
        _cookieConfiguration = cookieConfiguration;
    }

    // Insert refresh token into http-only cookie
    public void AddRefreshCookieToResponse(HttpResponse response, Guid refreshToken)
    {
        response.Cookies.Append(_cookieConfiguration.RefreshCookieName, refreshToken.ToString(),
            new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax });
    }

    // Extract refresh token from http-only cookie
    public Guid GetRefreshTokenFromCookie(HttpRequest request)
    {
        // Try to extract refresh token from cookie. If it is absent - exception
        if (!request.Cookies.TryGetValue(_cookieConfiguration.RefreshCookieName, out var refreshToken))
            throw new RefreshTokenException
            {
                Error = "refresh_token_not_found",
                ErrorDescription = "Expected refresh token in httponly cookie does not exists"
            };

        return Guid.Parse(refreshToken);
    }
}