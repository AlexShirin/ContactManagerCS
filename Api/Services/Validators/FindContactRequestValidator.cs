using ContactManagerCS.Services.Models;
using FluentValidation;

namespace ContactManagerCS.Services.Validators;

public class FindContactRequestValidator : AbstractValidator<FindContactRequest>
{
    public FindContactRequestValidator()
    {
        RuleFor(x => x.Keyword)
            .NotEmpty()
            .WithMessage("Please specify a non-empty Keyword");
    }
}
