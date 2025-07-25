using FluentValidation;
using UserMicroservice.Services.Dtos;

namespace UserMicroservice.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserdto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.firstName)
                .NotNull().WithMessage("First name cannot be null.")
                .MinimumLength(2).WithMessage("First name should be at least 2 characters.");

            RuleFor(x => x.last_name)
                .NotNull().WithMessage("Last name cannot be null.")
                .MinimumLength(2).WithMessage("Last name should be at least 2 characters.");

            RuleFor(x => x.password)
                .NotNull().WithMessage("Password cannot be null.")
                .MinimumLength(6).WithMessage("Password should be longer than 6 characters.")
                .Custom((value, context) =>
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
