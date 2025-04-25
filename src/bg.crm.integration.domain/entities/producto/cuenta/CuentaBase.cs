
namespace bg.crm.integration.application.entities.producto.cuenta;

public class CuentaBase
{
    public int Id_Cuenta { get; set; }
    public int Numero_Cuenta { get; set; }
    public int Id_Cliente { get; set; }
    public decimal Monto { get; set; }
    public decimal Disponible { get; set; }
}