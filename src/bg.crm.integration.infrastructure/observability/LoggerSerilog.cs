using Serilog;

namespace bg.crm.integration.infrastructure.observability
{
    public class LoggerSerilog : ILogger
    {
        public void Error(string messageTemplate, params object[]? propertyValues)
        {
            Log.Error(messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate, params object[]? propertyValues)
        {
            Log.Fatal(messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate, params object[]? propertyValues)
        {
            Log.Information(messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate, params object[]? propertyValues)
        {
            Log.Warning(messageTemplate, propertyValues);
        }
    }
}