using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace Microsoft.Identity.Firebase.Models
{
    public record FirebaseUiConfiguration
    {
        [JsonPropertyName("signInSuccessfulUrl")] public string signInSuccessfulUrl { get; private init; }
        [JsonPropertyName("privacyPolicyUrl")] public string privacyPolicyUrl { get; private init; }
        [JsonPropertyName("tosUrl")] public string tosUrl { get; private init; }

        public const string ConfigSectionName = "FirebaseUI";

        public FirebaseUiConfiguration(IConfiguration configuration) : this(configuration.GetSection(ConfigSectionName))
        {
        }

        public FirebaseUiConfiguration(IConfigurationSection configurationSection)
        {
            this.signInSuccessfulUrl = configurationSection["signInSuccessfulUrl"];
            this.privacyPolicyUrl = configurationSection["privacyPolicyUrl"];
            this.tosUrl = configurationSection["tosUrl"];
        }

        public string ToBase64Json() => Convert.ToBase64String(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(this));
    }
}
