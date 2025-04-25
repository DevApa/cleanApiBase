namespace bg.crm.integration.application.dtos.models.execptions
{
    public class NotFoundException : Exception
    {
        public List<string>? Errros { get; set; }
        public NotFoundException(string message, List<string>? errors = null) : base(message)
        {
            Errros = errors;
        }
    }
}