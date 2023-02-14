using Microsoft.AspNetCore.Http;
using TeachBoard.IdentityService.Application.Exceptions;

namespace TeachBoard.IdentityService.Application.Services;

public class CookieProvider
{
    // Insert refresh token into http-only cookie
    public void AddRefreshCookieToResponse(HttpResponse response, Guid refreshToken)
    {
        response.Cookies.Append("TeachBoard-Refresh-Token", refreshToken.ToString(),
            new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax });
    }

    // Extract refresh token from http-only cookie
    public Guid GetRefreshTokenFromCookie(HttpRequest request)
    {
        // Try to extract refresh token from cookie. If it is absent - exception
        if (!request.Cookies.TryGetValue("TeachBoard-Refresh-Token", out var refreshToken))
            throw new RefreshTokenException
            {
                Error = "refresh_token_not_found",
                ErrorDescription = "Expected refresh token in httponly cookie does not exists"
            };

        return Guid.Parse(refreshToken);
    }
}