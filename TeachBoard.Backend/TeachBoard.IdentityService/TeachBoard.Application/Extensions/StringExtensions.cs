using System.Security.Cryptography;
using System.Text;

namespace TeachBoard.Application.Extensions;

public static class StringExtensions
{
    public static string ToSha256(this string str)
    {
        // Create a SHA256   
        // ComputeHash - returns byte array  
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(str));

        // Convert byte array to a string   
        var builder = new StringBuilder();
        
        foreach (var t in bytes)
            builder.Append(t.ToString("x2"));
        
        return builder.ToString();
    }
}