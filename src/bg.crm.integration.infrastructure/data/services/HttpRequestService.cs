using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using Azure;
using bg.crm.integration.application.interfaces.services;
using bg.crm.integration.shared.extensions;
using Newtonsoft.Json;
using Serilog;

namespace bg.crm.integration.infrastructure.data.services
{
    public class HttpRequestService : IHttpRequestService, IServiceScoped
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;

        public HttpRequestService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        public async Task<TDestination> ExecuteRequest<TSource, TDestination>(
            string url,
            HttpMethod method,
            object? content = null,
            Dictionary<string, string>? headers = null,
            Dictionary<string, string>? queryParams = null,
            bool isFormEncoded = false,
            string? token = null,
            int timeout = 1500,
            Func<TSource, TDestination>? mapFunc = null,
            JsonSerializerSettings? jsonSettings = null,
            [CallerMemberName] string? callerName = null)
        {
            ValidateJobRequestParameters<TSource, TDestination>(url, method, content, timeout, mapFunc);
            Log.Information("Executing HTTP request: {Method} {Url}", method, url);
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeout);
            var requestMessage = BuildHttpRequestMessage(url, method, content, headers, queryParams, isFormEncoded, null, token);
            try
            {
                var responseMessage = await client.SendAsync(requestMessage).ConfigureAwait(false);
                var bodyResponse = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                Log.Information("Response: {StatusCode} {Body}", responseMessage.StatusCode, bodyResponse);

                responseMessage.EnsureSuccessStatusCode();

                var responseContent = JsonConvert.DeserializeObject<TSource>(bodyResponse, jsonSettings);

                var result = mapFunc != null ? mapFunc(responseContent!) : (TDestination)(object)responseContent!;

                return result;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        private void ValidateJobRequestParameters<TSource, TDestination>(
            string url, HttpMethod method,
            object? content,
            int timeout,
            Func<TSource, TDestination>? mapFunc,
            string? contentType = "")
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url), "URL cannot be null or empty.");
            if (method == HttpMethod.Post || method == HttpMethod.Put && content == null && contentType != "application/x-www-form-urlencoded")
                throw new ArgumentNullException(nameof(method), "HTTP method cannot be null.");
            if (timeout <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout must be greater than zero.");
            if (typeof(TSource) != typeof(TDestination) && mapFunc == null)
                throw new ArgumentNullException(nameof(mapFunc), "Mapping function cannot be null.");
        }

        private static HttpRequestMessage BuildHttpRequestMessage(
            string url,
            HttpMethod method,
            object? content,
            Dictionary<string, string>? headers,
            Dictionary<string, string>? queryParams,
            bool isFormEncoded,
            Dictionary<string, string>? fromData,
            string? token,
            string? contentType = "")
        {
            if (queryParams != null && queryParams.Any())
            {
                var queryString = string.Join("&", queryParams.Select(qp => $"{Uri.EscapeDataString(qp.Key)}={Uri.EscapeDataString(qp.Value)}"));
                url = url.Contains("?") ? $"{url}&{queryString}" : $"{url}?{queryString}";
            }

            var requestMessage = new HttpRequestMessage(method, url);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            if (content != null && (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch))
            {
                if (isFormEncoded)
                {
                    var formEncoded = new FormUrlEncodedContent((Dictionary<string, string>)content);
                    requestMessage.Content = formEncoded;
                }
                else
                {
                    var jsonContent = JsonConvert.SerializeObject(content);
                    requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }
            }
            return requestMessage;
        }
    }
}