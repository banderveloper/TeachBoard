using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TeachBoard.EducationService.WebApi.Models.Validation;

/// <summary>
/// Response validation error model
/// </summary>
public class ValidationResultModel  
{  
    public string Message { get; }
    public List<ValidationError> Errors { get; }  

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="modelState">Model state dictionary, contains invalid fields and description</param>
    public ValidationResultModel(ModelStateDictionary modelState)  
    {  
        Message = "Validation Failed";  
        Errors = modelState.Keys  
            .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))  
            .ToList();  
    }  
}  