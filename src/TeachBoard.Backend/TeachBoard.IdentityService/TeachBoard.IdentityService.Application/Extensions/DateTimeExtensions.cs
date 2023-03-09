namespace TeachBoard.IdentityService.Application.Extensions;

public static class DateTimeExtensions
{
    public static int ToUnixTimestamp(this DateTime dateTime)
    {
        return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }
}