namespace bg.crm.integration.application.dtos.models.execptions
{
    public class BadRequestException : Exception
    {
        public List<string>? Errros { get; set;}
        public BadRequestException(string message, List<string>? errors): base(message)
        {
            Errros = errors;
        }
    }
}