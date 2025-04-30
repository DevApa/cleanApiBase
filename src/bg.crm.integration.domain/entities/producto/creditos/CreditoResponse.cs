namespace bg.crm.integration.domain.entities.producto.creditos
{
    public class CreditoResponse
    {
        public int NumeroOperacion { get; set; }
        public string? TipoCredito { get; set; }
        public string? TipoMoneda { get; set; }
        public string? TipoTasa { get; set; }
        public string? TasaInteres { get; set; }
        public string? Monto { get; set; }
        public string? FechaDesembolso { get; set; }
        public string? FechaVencimiento { get; set; }
        public string? FechaUltimoPago { get; set; }
        public string? FechaUltimoEstado { get; set; }  
        public string? Canal { get; set; }
        public string? Estado { get; set; }
        public int PlazoDias { get; set; }
        public string? PlazoMeses { get; set; }
        public string? TasaInicial { get; set; }
        public string? TasaEfectiva { get; set; }
        public string? Cesantia { get; set; }
        public decimal? FactorReajuste { get; set; }
        public decimal? Comision { get; set; }
        public string? SeguroDesgravamen { get; set; }
        public string? TipoAmortizacion { get; set; }
        public int CuentaDesembolso { get; set; }
        public string? DestinoFondos { get; set; }
        public string? TipoGarantia { get; set; }
        public string? Aprobaciones { get; set; }
    }
}