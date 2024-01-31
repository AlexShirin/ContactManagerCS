using ContactManagerCS.Models;
using FluentValidation;

namespace ContactManagerCS.Validation;

public class FindContactRequestValidator : AbstractValidator<FindContactRequest>
{
    public FindContactRequestValidator()
    {
        RuleFor(x => x.Keyword)
            .NotEmpty()
            .WithMessage("Please specify a non-empty Keyword");
    }
}
