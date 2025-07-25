using CategoryMicroservice.Service.Dtos;
using FluentValidation;

namespace CategoryMicroservice.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("First name cannot be null.")
                .MinimumLength(2).WithMessage("First name should be at least 2 characters.");


        }
    }
}
