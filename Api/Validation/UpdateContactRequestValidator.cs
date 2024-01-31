using ContactManagerCS.Models;
using FluentValidation;

namespace ContactManagerCS.Validation;

public class UpdateContactRequestValidator : AbstractValidator<UpdateContactRequest>
{
    public UpdateContactRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Id must be positive integer");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Please specify a non-empty name")
            .MaximumLength(100)
            .WithMessage("Name length is 100 symbols max");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Please specify a non-empty email")
            .EmailAddress()
            .WithMessage("Email must be valid e-mail address")
            .MaximumLength(100)
            .WithMessage("Email length is 100 symbols max");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Please specify a non-empty phone")
            .Matches("""\d""")
            .WithMessage("Phone must contain only digits [0-9]")
            .MaximumLength(32)
            .WithMessage("Phone length is 32 digits max");

        RuleFor(x => x.Company)
            .MaximumLength(100)
            .WithMessage("Company length is 100 symbols max");
    }
}
