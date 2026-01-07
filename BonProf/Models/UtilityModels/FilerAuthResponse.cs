using System.Text.Json.Serialization;

namespace BonProf.Models;

    public class FilerAuthResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("expiresIn")]
        public long ExpiresIn { get; set; }
    }
