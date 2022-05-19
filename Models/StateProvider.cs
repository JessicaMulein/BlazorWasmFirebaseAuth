using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Identity.Firebase.Components;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace Microsoft.Identity.Firebase.Models
{
    public class StateProvider : AuthenticationStateProvider
    {
        private static StateProvider? _instance;

        /// <summary>
        /// Internal memoization to speed up repeat calls for identity
        /// </summary>
        private static Dictionary<int, ClaimsIdentity> _parsedJwts = new Dictionary<int, ClaimsIdentity>();

        public StateProvider()
        {
            if (_instance is not null)
                throw new InvalidOperationException("StateProvider is already initialized");
            _instance = this;
        }

        public static StateProvider Instance => _instance ?? throw new InvalidOperationException("StateProvider is not initialized");

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);
            try
            {
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                claims.AddRange(keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
            }
            catch (Exception e)
            {
            }

            return claims;
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public static ClaimsIdentity ClaimsIdentityFromFirebaseUser(FirebaseUser user)
        {
            var accessToken = user.StsTokenManager.AccessToken;
            var hasToken = !string.IsNullOrWhiteSpace(accessToken);
            var jwtHash = accessToken.GetHashCode();
            if (hasToken && _parsedJwts.ContainsKey(jwtHash))
            {
                return _parsedJwts[jwtHash];
            }
            var claims = ParseClaimsFromJwt(user.StsTokenManager.AccessToken).ToList();
            if (!claims.Any())
            {
                throw new InvalidOperationException("No claims found in JWT/Unable to parse claims from JWT");
            }
            var firstProvider = user.FirstProvider;
            if (firstProvider is null)
            {
                throw new InvalidOperationException("Missing provider data");
            }
            var result = new ClaimsIdentity(claims, firstProvider.ProviderId);
            _parsedJwts[jwtHash] = result;
            return result;
        }

        public static AuthenticationState AuthenticationStateFromUser(FirebaseUser? user)
        {
            try
            {
                if (user is not null)
                {
                    return new AuthenticationState(new ClaimsPrincipal(ClaimsIdentityFromFirebaseUser(user)));
                }

            }
            catch (Exception e)
            {

            }
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public static void InvokeNotifyAuthenticationStateChanged()
        {
            Instance.ManageUser();
        }

        public void ManageUser()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = FirebaseAuth.CurrentUser;
            var state = AuthenticationStateFromUser(user);
            return await Task.FromResult(state);
        }
    }
}
