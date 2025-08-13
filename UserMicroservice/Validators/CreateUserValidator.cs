using FluentValidation;
using classLib.UserDtos;

namespace UserMicroservice.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserdto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.email)
                .NotNull().WithMessage("email cannot be null.")
                .MinimumLength(2).WithMessage("First name should be at least 2 characters.");
            RuleFor(x => x.email).Custom((email, context) =>
            {
                var atIndex = email.IndexOf('@');
                if (atIndex <= 0 || atIndex != email.LastIndexOf('@'))
                {
                    context.AddFailure("Email", "'@' must be appeared at once.");
                    return;
                }

                var domainPart = email.Substring(atIndex + 1);
                if (string.IsNullOrWhiteSpace(domainPart) || !domainPart.Contains('.'))
                {
                    context.AddFailure("Email", "Domain part is needed");
                    return;
                }

                var domainParts = domainPart.Split('.');
                if (domainParts.Any(string.IsNullOrWhiteSpace))
                {
                    context.AddFailure("Email", "Extension is invalid.");
                }
            });


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
