using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions;

public static class ModelStateExtension
{
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
        var errors = new List<string>();
        foreach (var state in modelState.Values)
            errors.AddRange(state.Errors.Select(error => error.ErrorMessage));

        return errors;
    }
}
