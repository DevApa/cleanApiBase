namespace bg.crm.integration.application.dtos.models.productos.creditos
{
    public class CreditoRequestDto
    {
        public string? Masa { get; set; }
        public int CodigoCliente { get; set; }
        public int NumeroOperacion { get; set; }
        public string? FechaConcesion { get; set; }
    }
}