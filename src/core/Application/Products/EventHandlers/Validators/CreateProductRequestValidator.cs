using Application.Common.Validation;
using Application.Products.EventHandlers.Requests.CreateProduct;
using FluentValidation;
using Shared.Exceptions;

namespace Application.Products.EventHandlers.Validators;

public class CreateProductRequestValidator : CustomValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(sale => sale.Date)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage(ErrorsMessages.Validate.ValidateProduct.DateRequired);
    }
}