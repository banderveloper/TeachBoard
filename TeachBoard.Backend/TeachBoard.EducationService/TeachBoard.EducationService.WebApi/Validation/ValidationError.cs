using TeachBoard.EducationService.Application.Extensions;

namespace TeachBoard.EducationService.WebApi.Validation;

/// <summary>
/// Field validation error item
/// </summary>
public class ValidationError  
{
    public string? Field { get; }
    public string Message { get; }  

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="field">invalid property name</param>
    /// <param name="message">invalidity description</param>
    public ValidationError(string field, string message)  
    {  
        Field = field != string.Empty ? field.ToLowerFirstChar() : null;
        Message = message;  
    }  
}  
