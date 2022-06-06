using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    /// <summary>
    /// The checkout order command validator.
    /// </summary>
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        /// <summary>
        /// Initialization.
        /// </summary>
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(p => p.EmailAddress)
               .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        }
    }
}
