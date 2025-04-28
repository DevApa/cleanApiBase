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
            bool token = false,
            Dictionary<string, string>? tokenParams = null,
            int timeout = 1500,
            Func<TSource, TDestination>? mapFunc = null,
            JsonSerializerSettings? jsonSettings = null,
            [CallerMemberName] string? callerName = null,
            Dictionary<string, string>? fromData = null,
            string? contentType = "")
        {
            ValidateJobRequestParameters<TSource, TDestination>(url, method, content, timeout, mapFunc, contentType, token, tokenParams);
            Log.Information("Executing HTTP request: {Method} {Url}", method, url);

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeout);

            var requestMessage = BuildHttpRequestMessage(url, method, content, headers, queryParams, isFormEncoded, fromData, contentType);
            try
            {
                if (token && tokenParams != null)
                {
                    Task<string> tokenTask = _tokenService.GetTokenAsync(tokenParams);
                    var tokenValue = await tokenTask.ConfigureAwait(false);
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenValue);
                }

                var responseMessage = await client.SendAsync(requestMessage).ConfigureAwait(false);
                var bodyResponse = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                Log.Information("Response: {StatusCode} {Body}", responseMessage.StatusCode, bodyResponse);

                responseMessage.EnsureSuccessStatusCode();

                var responseContent = JsonConvert.DeserializeObject<TSource>(bodyResponse, jsonSettings);

                var result = mapFunc != null ? mapFunc(responseContent!) : (TDestination)(object)responseContent!;

                return result;
            }
            catch (TaskCanceledException ex)
            {
                Log.Error(ex, "La solicitud [{method}] desde {Caller} ha excedido el tiempode espera", method, callerName);
                throw new TimeoutException($"Request to {url} timed out after {timeout} milliseconds.", ex);
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "Error en la solicitud [{method}] desde {Caller}: ", method, callerName);
                throw new HttpRequestException($"Error al realizar la solicitud HTTP: {ex.Message} ", ex);
            }
            catch (JsonSerializationException ex)
            {
                Log.Error(ex, "JSON serialization error: {Url}", url);
                throw new JsonSerializationException($"Error al intentar deserializar response de la {url}.", ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Se produjo un error inesperado: {Url}", url);
                throw new Exception($"Se produjo un error inesperado al procesar la solicitud para {url}.", ex);
            }
            finally
            {
                requestMessage.Dispose();
                Log.Information("[{method}] {Caller} {url}", method, callerName, url);
            }
        }

        #region Validation Parameters
        private void ValidateJobRequestParameters<TSource, TDestination>(
            string url,
            HttpMethod method,
            object? content,
            int timeout,
            Func<TSource, TDestination>? mapFunc,
            string? contentType = "",
            bool token = false,
            Dictionary<string, string>? tokenParams = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url), "La dirección URL no puede ser nula o vacía.");
            if (method == null)
                throw new ArgumentNullException(nameof(method), "El método HTTP no puede ser vacío.");
            if (method == HttpMethod.Post || method == HttpMethod.Put && content == null && contentType != "application/x-www-form-urlencoded")
                throw new ArgumentNullException(nameof(method), "El método HTTP no puede ser vacío.");
            if (timeout <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeout), "El tiempo de espera tiene que ser mayor que cero.");
            if (typeof(TSource) != typeof(TDestination) && mapFunc == null)
                throw new ArgumentNullException(nameof(mapFunc), "La función de conversión no puede ser nula.");
            if (token && tokenParams == null)
                throw new ArgumentNullException(nameof(tokenParams), "Los parámetros de token no pueden ser nulos.");
        }
        #endregion

        #region Building Sin Token
        private static HttpRequestMessage BuildHttpRequestMessage(
            string url,
            HttpMethod method,
            object? content,
            Dictionary<string, string>? headers,
            Dictionary<string, string>? queryParams,
            bool isFormEncoded,
            Dictionary<string, string>? fromData = null,
            string? contentType = ""
           )
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
        #endregion

    }
}