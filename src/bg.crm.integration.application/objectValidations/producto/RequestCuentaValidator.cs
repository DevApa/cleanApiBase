using bg.crm.integration.application.dtos.models.productos.creditos;
using bg.crm.integration.domain.entities.producto.cuenta;
using FluentValidation;

namespace bg.crm.integration.application.objectValidations.producto
{
    internal class RequestCuentaValidator : AbstractValidator<CreditoRequestDto>
    {
        public RequestCuentaValidator()
        {
            RuleFor(x => x.Masa)
            .NotEmpty().WithMessage(masa => string.Format("", nameof(masa)));
        }
    }
}