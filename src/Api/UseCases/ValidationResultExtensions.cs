using FluentValidation.Results;

namespace Api.UseCases;

public static class ValidationResultExtensions
{
    public static IEnumerable<ErrorMessage> ToErrorMessages(
        this ValidationResult validationResult)
    {
        return validationResult.Errors
            .Select(error => 
                new ErrorMessage($"{error.PropertyName}{error.ErrorMessage}"));
    }
}