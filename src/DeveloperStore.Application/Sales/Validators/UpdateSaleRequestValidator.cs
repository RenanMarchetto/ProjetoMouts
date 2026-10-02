using DeveloperStore.Application.Sales.Requests;
using FluentValidation;

namespace DeveloperStore.Application.Sales.Validators
{
    public sealed class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {

        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("CustomerName is required.")
                .MaximumLength(200).WithMessage("CustomerName must be less than or equal to 200 characters.");
            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("BranchId is required.");
            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("BranchName is required.")
                .MaximumLength(200).WithMessage("BranchName must be less than or equal to 200 characters.");
            RuleFor(x => x.Items).NotEmpty().WithMessage("Sale must contain at least one item.");
            RuleForEach(x => x.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
                    item.RuleFor(x => x.ProductName)
                        .NotEmpty().WithMessage("ProductName is required.")
                        .MaximumLength(200).WithMessage("ProductName must be less than or equal to 200 characters.");
                    item.RuleFor(x => x.Quantity)
                        .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                        .LessThanOrEqualTo(20).WithMessage("Quantity must be less than or equal to 20.");
                    item.RuleFor(x => x.UnitPrice)
                        .GreaterThan(0).WithMessage("UnitPrice must be greater than 0.");
                }
                );

        }
    }
}
