namespace bg.crm.integration.infrastructure.observability
{
    public interface ILogger
    {
        void Information(string messageTemplate, params object[]? propertyValues);
        void Error(string messageTemplate, params object[]? propertyValues);
        void Warning(string messageTemplate, params object[]? propertyValues);
        void Fatal(string messageTemplate, params object[]? propertyValues);
    }
}