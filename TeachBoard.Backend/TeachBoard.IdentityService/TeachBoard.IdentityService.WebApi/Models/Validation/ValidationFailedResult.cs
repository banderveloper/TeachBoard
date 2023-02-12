using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TeachBoard.IdentityService.WebApi.Models.Validation;

// 422 status code instead of default 400 after validation error  
public class ValidationFailedResult : ObjectResult  
{  
    public ValidationFailedResult(ModelStateDictionary modelState)  
        : base(new ValidationResultModel(modelState))  
    {  
        StatusCode = StatusCodes.Status422UnprocessableEntity; //change the http status code to 422.  
    }  
}  