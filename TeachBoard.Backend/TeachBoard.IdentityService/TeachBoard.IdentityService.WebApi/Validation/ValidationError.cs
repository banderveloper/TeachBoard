using TeachBoard.IdentityService.Application.Extensions;

namespace TeachBoard.IdentityService.WebApi.Validation;

// Field validation error item
public class ValidationError  
{
    public string? Field { get; }
    public string Message { get; }  

    public ValidationError(string field, string message)  
    {  
        Field = field != string.Empty ? field.ToLowerFirstChar() : null;
        Message = message;  
    }  
}  
