namespace bg.crm.integration.domain.entities.catalogo
{
    public class CatalogoCabecera
    {
        public int Pagina { get; set; }
        public bool MasPaginas { get; set; }
        public List<CatalogoItem>? Catalogos { get; set; }
    }
}