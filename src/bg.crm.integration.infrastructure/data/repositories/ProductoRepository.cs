using AutoMapper;
using bg.crm.integration.application.interfaces.services;
using bg.crm.integration.domain.entities.producto.cuenta;
using bg.crm.integration.shared.extensions;
using Microsoft.Extensions.Configuration;

namespace bg.crm.integration.infrastructure.data.repositories
{
    public class ProductoRepository : IProductoRepository, IServiceScoped
    {
        private readonly IHttpRequestService _httpRequestService;
        private readonly IConfiguration _Configuration;
        private readonly IMapper _mapper;

        public ProductoRepository(IHttpRequestService httpRequestService, ITokenService tokenService, IConfiguration configuration, IMapper mapper)
        {
            _httpRequestService = httpRequestService;
            _Configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseCuenta> ConsultaCuenta(RequestCuenta request)
        {
            var uri = $"{_Configuration["InfraConfig:Micros:Productos:urlService"]!}/api/Producto/ConsultaCuenta";
            var Parameters = new Dictionary<string, string>
            {
                { "resource", _Configuration["InfraConfig:Micros:Productos:resource"]! },
                { "client_secret", _Configuration["InfraConfig:Micros:Productos:client_secret"]! },
                { "client_id", _Configuration["InfraConfig:Micros:Productos:client_id"]! }
            };

            var response = await _httpRequestService.ExecuteRequest<RequestCuenta, ResponseCuenta>(
                uri,
                HttpMethod.Get,
                request,
                null,
                null,
                false,
                true,
                Parameters,
                mapFunc: source=>_mapper.Map<ResponseCuenta>(source)
            );
            
            return response;
        }
    }
}