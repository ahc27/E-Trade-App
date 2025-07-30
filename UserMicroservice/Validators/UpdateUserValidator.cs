using FluentValidation;
using UserMicroservice.Services.Dtos;

namespace UserMicroservice.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserdto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.firstName)
                    .NotNull().WithMessage("First name cannot be null.")
                    .NotEmpty().WithMessage("First name  cannot be empty.")
                    .MinimumLength(2).WithMessage("First name should be at least 2 characters.");

            RuleFor(x => x.lastName)
                    .NotNull().WithMessage("Last name cannot be null.")
                    .NotEmpty().WithMessage("Last name cannot be empty.")
                    .MinimumLength(2).WithMessage("Last name should be at least 2 characters.");

            RuleFor(x => x.password)
                    .NotNull().WithMessage("Password cannot be null.")
                    .NotEmpty().WithMessage("Password cannot be empty.")
                    .MinimumLength(6).WithMessage("Password should be longer than 6 characters.");

            RuleFor(x => x.password).Custom((value, context) =>
            {
             /*   if (value != null && !value.Any(char.IsUpper))
                {
                    context.AddFailure("Password", "Password must contain at least one uppercase letter.");
                }
                if (value != null && !value.Any(char.IsLower))
                {
                    context.AddFailure("Password", "Password must contain at least one lowercase letter.");
                }*/
                if (value != null && !value.Any(char.IsDigit))
                {
                    context.AddFailure("Password", "Password must contain at least one digit.");
                }
            });
        }
    }
}
