namespace bg.crm.integration.application.dtos.responses
{
    public class MsDtoResponseError
    {
        public int Code { get; set; }
        public string? TraceId { get; set; }
        public string? Message { get; set; }
        public List<MsError>? Errors { get; set; }
    }
}