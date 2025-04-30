using AutoMapper;
using bg.crm.integration.application.interfaces.services;
using bg.crm.integration.domain.entities.catalogo;
using bg.crm.integration.shared.extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace bg.crm.integration.infrastructure.data.services
{
    public class CatalogoService : ICatalogoService, IServiceScoped
    {
        private readonly IHttpRequestService _httpRequestService;
        private readonly IConfiguration _Configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromDays(1);


        public CatalogoService(IHttpRequestService httpRequestService, IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _httpRequestService = httpRequestService;
            _Configuration = configuration;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<CatalogoCabecera> ConsultarCatalogoAsync(int IdCatTipo, int? nroPagina = null, int? idCodigoDetalle = null, string? codBancoCore = null, string? codBancoNeo = null, string? codBancoGoldenRec = null, string? codBancoOtros = null)
        {
            throw new NotImplementedException();
        }

        public async Task<CatalogoGeneralResponse> ConsultarCatalogoGeneralAsync(CatalogoGeneralRequest request)
        {
            var uri = $"{_Configuration["InfraConfig:Micros:Catalogo:urlService"]!}/api/Catalogo/ConsultaCatalogoGeneral";
            var header = new Dictionary<string, string>
            {
                { "tipo", request.Tipo! },
                { "pagina", request.Pagina! },
                { "codigo", request.Codigo! }
            };

            throw new NotImplementedException();
        }

        public async Task<CatalogoItem> ConsultarCatalogoItemAsync(int IdCatTipo, int? idCodigoDetalle = null, string? codBancoCore = null, string? codBancoNeo = null, string? codBancoGoldenRec = null, string? codBancoOtros = null)
        {
            throw new NotImplementedException();
        }
    }
}