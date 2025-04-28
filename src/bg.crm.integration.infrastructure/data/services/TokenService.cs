using System.Reflection.Metadata.Ecma335;
using bg.crm.integration.application.interfaces.services;
using bg.crm.integration.domain.entities.Auth;
using bg.crm.integration.shared.extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace bg.crm.integration.infrastructure.data.services
{
    public class TokenService : ITokenService, IServiceScoped
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private const string? CacheKey = "TokenCacheKey";

        public TokenService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public async Task<string> GetTokenAsync(Dictionary<string, string>? tokenParams)
        {
            if (_memoryCache.TryGetValue(CacheKey!, out string? token))
                token = await RefreshTokenAsync(tokenParams!);
            return token!;
        }

        private async Task<string> RefreshTokenAsync(Dictionary<string, string> tokenParams)
        {
            var url = $"{_configuration["Authentication:url"]}outh2/token";
            using var client = _httpClientFactory.CreateClient();

            var body = new Dictionary<string, string>
            {
                { "resource", tokenParams["resource"] },
                { "client_secret", tokenParams["client_secret"] },
                { "grant_type", "client_credentials" },
                { "client_id", tokenParams["client_id"] }
            };

            var content = new FormUrlEncodedContent(body);

            try
            {
                HttpResponseMessage responseMessage = await client.PostAsync(url, content).ConfigureAwait(false);

                responseMessage.EnsureSuccessStatusCode();

                var responseBody = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);

                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    _memoryCache.Set(CacheKey!, tokenResponse.AccessToken, TimeSpan.FromSeconds(int.Parse(tokenResponse.ExpiresIn!) - 60));
                    return tokenResponse.AccessToken;
                }
                else                
                    throw new Exception("Token response is null or access token is empty.");                
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error en la solicitud HTTP", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado token", ex);
            }
        }
    }
}