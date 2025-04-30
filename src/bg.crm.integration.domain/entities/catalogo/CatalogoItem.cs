namespace bg.crm.integration.domain.entities.catalogo
{
    public class CatalogoItem
    {
        public int IdCatTipo { get; set; }
        public string? DescIdcatTipo { get; set; }
        public int IdCodigoCatDetalle { get; set; }
        public string? DescripcionCatalogo { get; set; }
        public string? CatalogoPadre { get; set; }
        public string? DescripcionCatalogoPadre { get; set; }
        public int IdDetalleCatalogoPadre { get; set; }
        public string? DescripcionPadre { get; set; }
        public string? CodBancoCore { get; set; }
        public string? CodBancoNeo { get; set; }
        public string? CodBancoGoldenRec { get; set; }
        public string? FerchaModificacion { get; set; }
    }
}