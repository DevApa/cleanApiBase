using bg.crm.integration.domain.entities.catalogo;

namespace bg.crm.integration.application.interfaces.services
{
    public interface ICatalogoService
    {
        Task<CatalogoGeneralResponse> ConsultarCatalogoGeneralAsync(CatalogoGeneralRequest request);
        Task<CatalogoCabecera> ConsultarCatalogoAsync(int IdCatTipo, int? nroPagina=null, int? idCodigoDetalle= null, string? codBancoCore=null, string? codBancoNeo=null, string? codBancoGoldenRec=null, string? codBancoOtros= null);
        Task<CatalogoItem> ConsultarCatalogoItemAsync(int IdCatTipo, int? idCodigoDetalle= null, string? codBancoCore=null, string? codBancoNeo=null, string? codBancoGoldenRec=null, string? codBancoOtros= null);
    }
}