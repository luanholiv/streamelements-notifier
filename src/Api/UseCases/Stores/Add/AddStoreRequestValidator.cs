using FluentValidation;

namespace Api.UseCases.Stores.Add;

public sealed class AddStoreRequestValidator : AbstractValidator<AddStoreRequest>
{
    public AddStoreRequestValidator()
    {
        RuleFor(request => request.ExternalId).NotEmpty().MaximumLength(24);
        RuleFor(request => request.StreamerName).NotEmpty().MaximumLength(100);
        RuleFor(request => request.Uri).NotEmpty().MaximumLength(100);
    }
}