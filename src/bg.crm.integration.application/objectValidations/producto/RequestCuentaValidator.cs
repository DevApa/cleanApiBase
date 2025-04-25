using bg.crm.integration.domain.entities.producto.cuenta;
using FluentValidation;

namespace bg.crm.integration.application.objectValidations.producto
{
    internal class RequestCuentaValidator : AbstractValidator<RequestCuenta>
    {
        public RequestCuentaValidator()
        {
            RuleFor(x=>x.Masa)
            .NotEmpty().WithMessage(masa => string.Format("", nameof(masa)));
        }
    }
}