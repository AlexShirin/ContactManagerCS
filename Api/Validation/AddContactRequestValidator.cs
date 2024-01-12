using ContactManagerCS.Models;

using FluentValidation;

namespace ContactManagerCS.Validation
{
    public class AddContactRequestValidator : AbstractValidator<AddContactRequest>
    {
        public AddContactRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Please specify a unique id");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Please specify a non-empty name");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Please specify a non-empty email");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Please specify a non-empty phone");

            RuleFor(x => x.Work)
                .NotEmpty()
                .WithMessage("Please specify a non-empty work name");
        }
    }
}
