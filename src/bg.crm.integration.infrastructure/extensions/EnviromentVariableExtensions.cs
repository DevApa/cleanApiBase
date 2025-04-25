using System.Collections;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace bg.crm.integration.infrastructure.extensions
{
    public static class EnviromentVariableExtensions
    {
        public static WebApplicationBuilder ConfigureEnviromentVariables(this WebApplicationBuilder builder, string appName)
        {
            var _appName = $"{appName}VARIABLES";
            var values = new Dictionary<string, string>();
            foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
            {
                if(env.Key!=null)
                {
                    var key = env.Key.ToString()!.Trim();
                    if (key.StartsWith($"{_appName}_"))
                    {
                        var _name = key.Replace($"{_appName}_", "");
                        try
                        {
                            if (env.Value != null)                            
                               values.Add(_name, env.Value.ToString()!);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error parsing environment variable {key}: {ex.Message}", ex);
                        }
                    }
                }                
            }
            builder.Configuration.AddInMemoryCollection(values.Select(kv => new KeyValuePair<string, string?>(kv.Key, kv.Value)));
            
            return builder;
        }
    }
}