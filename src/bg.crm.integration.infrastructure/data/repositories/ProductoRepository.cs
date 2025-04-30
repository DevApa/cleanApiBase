using AutoMapper;
using bg.crm.integration.application.dtos.models;
using bg.crm.integration.application.dtos.models.execptions;
using bg.crm.integration.application.dtos.models.productos.creditos;
using bg.crm.integration.application.dtos.responses;
using bg.crm.integration.application.interfaces.services;
using bg.crm.integration.domain.entities.producto.creditos;
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

        public async Task<ServiceResponseDto<CreditoResponseDto>> ConsultarResumenCreditoRepositoryAsync(CreditoRequestDto request)
        {
            var uri = $"{_Configuration["InfraConfig:Micros:Productos:urlService"]!}/api/Producto/ConsultaCuenta";
            var Parameters = new Dictionary<string, string>
            {
                { "resource", _Configuration["ServConfig:Auth:resource"]! },
                { "client_secret", _Configuration["ServConfig:Auth:client_secret"]! },
                { "client_id", _Configuration["ServConfig:Auth:client_id"]! }
            };

            var response = await _httpRequestService.ExecuteRequest<MsDtoResponseSuccess<CreditoResponse>, CreditoResponseDto>(
                uri,
                HttpMethod.Get,
                request,
                null,
                null,
                false,
                true,
                Parameters,
                mapFunc: source => _mapper.Map<CreditoResponseDto>(source.Data!)
            );

            return (response != null) ? new ServiceResponseDto<CreditoResponseDto>
            {
                CodigoRetorno = 0,
                MensajeRetorno = "Consulta exitosa",
                ServiceResponse = response
            }
            : new ServiceResponseDto<CreditoResponseDto>
            {
                CodigoRetorno = 1,
                MensajeRetorno = "La consulta no devolvió resultados",
                ServiceResponse = new()
            };
        }
    }
}