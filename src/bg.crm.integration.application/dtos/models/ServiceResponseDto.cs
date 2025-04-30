namespace bg.crm.integration.application.dtos.models
{
    public class ServiceResponseDto<T>
    {
        public int CodigoRetorno { get; set; }
        public string? MensajeRetorno { get; set; }
        public required T ServiceResponse { get; set; }
    }
}