using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Identity.Firebase.Models
{
    public record FirebaseUiConfiguration
    {
        [JsonPropertyName("signInSuccessfulUrl")] public string signInSuccessfulUrl { get; private init; }
        [JsonPropertyName("privacyUrl")] public string privacyUrl { get; private init; }
        [JsonPropertyName("termsOfServiceUrl")] public string termsOfServiceUrl { get; private init; }

        public const string ConfigSectionName = "FirebaseUI";

        public FirebaseUiConfiguration(IConfiguration configuration) : this(configuration.GetSection(ConfigSectionName))
        {
        }

        public FirebaseUiConfiguration(IConfigurationSection configurationSection)
        {
            this.signInSuccessfulUrl = configurationSection["signInSuccessfulUrl"];
            this.privacyUrl = configurationSection["privacyUrl"];
            this.termsOfServiceUrl = configurationSection["termsOfServiceUrl"];
        }

        public string ToBase64Json() => Convert.ToBase64String(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(this));
    }
}
