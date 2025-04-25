
using bg.crm.integration.domain.entities.producto.cuenta;

namespace bg.crm.integration.application.interfaces.services
{
    public interface IProductoService
    {
        Task<ResponseCuenta> ConsultaCuenta(RequestCuenta request);
    }
}