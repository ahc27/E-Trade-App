using classLib.ProductDtos;
using FluentValidation;

namespace Products.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.name)
                .NotNull().WithMessage("Name cannot be null.")
                .MinimumLength(2).WithMessage("First name should be at least 2 characters.");
            RuleFor(x => x.price)
                .GreaterThan(0).WithMessage("You cannot submit a free product.");
            RuleFor(x => x.stockQuantity)
                .NotNull()
                .GreaterThan(0).WithMessage("Enter a legit number of product");
        }
    }
}
