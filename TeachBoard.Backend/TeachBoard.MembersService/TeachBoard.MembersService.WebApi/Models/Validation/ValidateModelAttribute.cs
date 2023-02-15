using Microsoft.AspNetCore.Mvc.Filters;
using TeachBoard.MembersService.WebApi.Models.Validation;

namespace TeachBoard.MembersService.Application.Validation;

/// <summary>
/// Attribute for enabling custom validation model response
/// </summary>
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
            context.Result = new ValidationFailedResult(context.ModelState);
    }
}