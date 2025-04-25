using bg.crm.integration.application.interfaces.services;
using bg.crm.integration.domain.entities.producto.cuenta;
using bg.crm.integration.shared.extensions;

namespace bg.crm.integration.infrastructure.data.repositories
{
    public class ProductoRepository : IProductoRepository, IServiceScoped
    {
        private readonly IHttpRequestService _httpRequestService;
        private readonly ITokenService _tokenService;

        public ProductoRepository(IHttpRequestService httpRequestService, ITokenService tokenService)
        {
            _httpRequestService = httpRequestService;
            _tokenService = tokenService;
        }

        public async Task<ResponseCuenta> ConsultaCuenta(RequestCuenta request)
        {
            throw new NotImplementedException();
        }
    }
}