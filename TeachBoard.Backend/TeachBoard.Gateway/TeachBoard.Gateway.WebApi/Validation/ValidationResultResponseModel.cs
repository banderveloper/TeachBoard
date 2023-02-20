using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TeachBoard.Gateway.WebApi.Validation;

// Returning validation error model
public class ValidationResultModel  
{  
    public string Message { get; }
    public List<ValidationError> Errors { get; }  

    public ValidationResultModel(ModelStateDictionary modelState)  
    {  
        Message = "Validation Failed";  
        Errors = modelState.Keys  
            .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))  
            .ToList();  
    }  
}  