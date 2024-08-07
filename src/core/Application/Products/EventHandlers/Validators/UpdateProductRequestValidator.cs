using Application.Common.Validation;
using Application.Products.EventHandlers.Requests.UpdateProduct;
using FluentValidation;

namespace Application.Products.EventHandlers.Validators;

public class UpdateProductRequestValidator : CustomValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        
        RuleFor(p => p.Id)
            .NotEmpty()
            .WithMessage("O ID � de preenchimento obrigat�rio.");

        RuleFor(p => p.Status)
            .NotEmpty()
            .WithMessage("O status � de preenchimento obrigat�rio.");
    }
}