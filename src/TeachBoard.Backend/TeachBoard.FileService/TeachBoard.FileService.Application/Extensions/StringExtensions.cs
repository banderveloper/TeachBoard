namespace TeachBoard.FileService.Application.Extensions;

public static class StringExtensions
{
    public static string ToLowerFirstChar(this string input)
    {
        if(string.IsNullOrEmpty(input))
            return input;

        return char.ToLower(input[0]) + input.Substring(1);
    }
}