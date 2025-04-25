namespace bg.crm.integration.application.dtos.models.productos.tarjetas
{
    public class TarjetaCreditoDto
    {
        public int Bin { get; set; }
        public long NumeroTarjeta { get; set; }
        public string? Nombre { get; set; }
        public int FechaExpedicion { get; set; }
        public int FechaExpiracion { get; set; }
        public string? NumeroEnmascarado { get; set; }
        public int SecureCode { get; set; }
    }
}