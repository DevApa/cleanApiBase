using System.Text.Json;
using Newtonsoft.Json;

namespace bg.crm.integration.domain.entities.Auth
{
    public class TokenResponse
    {
        [JsonProperty("token_type")]
        public string? TokenType { get; set; }
        [JsonProperty("expires_in")]
        public string? ExpiresIn { get; set; }
        [JsonProperty("ext_expire_in")]
        public string? ExtExpireIn { get; set; }
        [JsonProperty("expire_on")]
        public string? ExpireOn { get; set; }
        [JsonProperty("not_before")]
        public string? NotBefore { get; set; }
        [JsonProperty("resource")]
        public string? Resource { get; set; }
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
    }
}