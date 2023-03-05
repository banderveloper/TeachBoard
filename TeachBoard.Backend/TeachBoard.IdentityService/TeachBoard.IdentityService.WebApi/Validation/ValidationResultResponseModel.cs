using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TeachBoard.IdentityService.WebApi.Validation;

// Returning validation error model
public class ValidationResultModel
{
    public string ErrorCode { get; } = "validation_failed";
    public IList<ValidationError> InvalidFields { get; }

    public ValidationResultModel(ModelStateDictionary modelState)
    {
        InvalidFields = modelState.Keys.SelectMany(key =>
                modelState[key].Errors.Select(x =>
                    new ValidationError(key.Replace("$.", ""),
                        x.ErrorMessage)))
            .ToList();
    }
}