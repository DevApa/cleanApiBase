using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace bg.crm.integration.application.interfaces.services
{
    public interface IHttpRequestService
    {
        Task<TDestination> ExecuteRequest<TSource, TDestination>(
            string url,
            HttpMethod method,
            object? content = null,
            Dictionary<string, string>? headers = null,
            Dictionary<string, string>? queryParams = null,
            bool isFormEncoded = false,
            bool token = false,
            Dictionary<string, string>? tokenParams = null,
            int timeout = 1500,
            Func<TSource, TDestination>? mapFunc = null,
            JsonSerializerSettings? jsonSettings = null,
            [CallerMemberName] string? callerName = null,
            Dictionary<string, string>? fromData = null,
            string? contentType = ""
            );
    }
}