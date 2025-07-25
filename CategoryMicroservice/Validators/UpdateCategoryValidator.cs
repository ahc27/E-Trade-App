using CategoryMicroservice.Service.Dtos;
using FluentValidation;

namespace CategoryMicroservice.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("First name cannot be null.")
                .MinimumLength(2).WithMessage("First name should be at least 2 characters.");


        }

    }
}
