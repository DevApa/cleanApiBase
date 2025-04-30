
using bg.crm.integration.application.dtos.models;
using bg.crm.integration.application.dtos.models.productos.creditos;

namespace bg.crm.integration.application.interfaces.services
{
    public interface IProductoService
    {
        Task<ServiceResponseDto<CreditoResponseDto>> ConsultarResumenCreditoServiceAsync(CreditoRequestDto request);
    }
}