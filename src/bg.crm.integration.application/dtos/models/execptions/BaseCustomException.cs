namespace bg.crm.integration.application.dtos.models.execptions
{
    public class BaseCustomException : Exception
    {
        public int Code { get; set; }
        public override string? StackTrace { get; }

        public BaseCustomException(string message, string? stackTrace, int code) : base(message)
        {
            Code = code;
            StackTrace = stackTrace;
        }
    }
}