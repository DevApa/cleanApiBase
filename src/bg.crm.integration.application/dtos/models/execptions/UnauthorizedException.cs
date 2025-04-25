namespace bg.crm.integration.application.dtos.models.execptions
{
    public class UnauthorizedException : Exception
    {
        public List<string>? Errros { get; set; }
        public UnauthorizedException(string message, List<string>? errors) : base(message)
        {
            Errros = errors;
        }
    }
}