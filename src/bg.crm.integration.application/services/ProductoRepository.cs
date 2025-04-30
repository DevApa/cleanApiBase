using bg.crm.integration.application.dtos.models;
using bg.crm.integration.application.dtos.models.execptions;
using bg.crm.integration.application.dtos.models.productos.creditos;
using bg.crm.integration.application.interfaces.services;
using bg.crm.integration.shared.extensions;
using FluentValidation;

namespace bg.crm.integration.application.services
{
    public class ProductoRepository : IProductoService, IServiceScoped
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IValidator<CreditoRequestDto> _cuentaValidator;

        public ProductoRepository(
            IProductoRepository productoRepository,
            IValidator<CreditoRequestDto> cuentaValidator
            )
        {
            _productoRepository = productoRepository;
            _cuentaValidator = cuentaValidator;
        }


        public async Task<ServiceResponseDto<CreditoResponseDto>> ConsultarResumenCreditoServiceAsync(CreditoRequestDto request)
        {
            var parameters = _cuentaValidator.Validate(request);
            if (!parameters.IsValid)
                throw new BadRequestException(string.Empty, parameters.Errors.Select(x => x.ErrorMessage).ToList());



            var response = await _productoRepository.ConsultarResumenCreditoRepositoryAsync(request);

            return response ?? throw new NotFoundException("No se encontraron resultados para la consulta de cuenta.");
        }
    }
}