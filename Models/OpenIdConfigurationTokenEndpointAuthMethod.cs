using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Microsoft.Identity.Firebase.Models
{
    /// <summary>
    ///     Valid elements for token_endpoint_auth_methods_supported in OpenID Configuration
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OpenIdConfigurationTokenEndpointAuthMethod
    {
        [EnumMember(Value = "client_secret_basic")]
        ClientSecretBasic,

        [EnumMember(Value = "client_secret_post")]
        ClientSecretPost,

        [EnumMember(Value = "client_secret_jwt")]
        ClientSecretJwt,

        [EnumMember(Value = "private_key_jwt")]
        PrivateKeyJwt,

        [EnumMember(Value = "tls_client_auth")]
        TlsClientAuth
    }
}
