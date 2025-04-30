using bg.crm.integration.application.dtos.models;
using bg.crm.integration.application.dtos.models.productos.creditos;

namespace bg.crm.integration.application.interfaces.services
{
    public interface IProductoRepository
    {
        Task<ServiceResponseDto<CreditoResponseDto>> ConsultarResumenCreditoRepositoryAsync(CreditoRequestDto request);
    }
}