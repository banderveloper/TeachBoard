using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TeachBoard.MembersService.WebApi.Models.Validation;

/// <summary>
/// 422 status code insteal of default 400 after validation error
/// </summary>
public class ValidationFailedResult : ObjectResult  
{  
    public ValidationFailedResult(ModelStateDictionary modelState)  
        : base(new ValidationResultModel(modelState))  
    {  
        StatusCode = StatusCodes.Status422UnprocessableEntity; //change the http status code to 422.  
    }  
}  