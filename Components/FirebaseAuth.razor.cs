using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Firebase.Models;
using Microsoft.Identity.Firebase.Models.Account;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Identity.Firebase.Components
{
    public partial class FirebaseAuth : ComponentBase
    {
        [Inject] private static IJSRuntime StaticJsInterop { get; set; }

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject] private static NavigationManager StaticNavigationManager { get; set; }

        [Bindable(true)]
        public static bool IsAuthenticated => CurrentUser is FirebaseUser;

        [Bindable(true)]
        public static FirebaseUser? CurrentUser { get; private set; }

        private static bool Initialized { get; set; } = false;

        public static FirebaseAuth? Instance { get; private set; }

        public DotNetObjectReference<FirebaseAuth> InstanceReference => DotNetObjectReference.Create(this);

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!Initialized && this._jsRuntime is not null)
            {
                Instance = this;
                await this._jsRuntime.InvokeVoidAsync("window.firebaseInitialize", this.InstanceReference);
                Initialized = true;
            }
        }

        [JSInvokable]
        public async Task OnAuthStateChanged(FirebaseUser? user)
        {
            CurrentUser = user;
            StateProvider.InvokeNotifyAuthenticationStateChanged();
        }

        public static async Task<FirebaseUser> FirebaseUserFromJsonDataAsync(string userData)
        {
            if (string.IsNullOrEmpty(userData))
                throw new ArgumentNullException(nameof(userData));

            var userDataBytes = userData.ToCharArray().Select(c => (byte)c).ToArray();
            var userDataStream = new MemoryStream(userDataBytes);
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                IgnoreReadOnlyFields = false,
                IgnoreReadOnlyProperties = false
            };
            FirebaseUser? userObject = null;
            try
            {
                userObject = await JsonSerializer.DeserializeAsync<FirebaseUser>(
                    utf8Json: userDataStream,
                    options: jsonSerializerOptions);
            }
            catch (Exception e)
            {
                userObject = null;
            }

            if (userObject is null)
                throw new InvalidOperationException("Unable to deserialize user data.");

            return userObject;
        }

        public static async Task<FirebaseUser?> SignInEmailUserAsync(string email, string password, IJSRuntime? jsRuntime = null)
        {
            if (jsRuntime is not null)
            {
                StaticJsInterop = jsRuntime;
            }
            var userData = await StaticJsInterop!.InvokeAsync<string?>("window.firebaseLoginUser", email, password);
            if (string.IsNullOrEmpty(userData))
                return null;
            return await FirebaseUserFromJsonDataAsync(userData);
        }


        public static async Task<FirebaseUser?> CreateEmailUserAsync(NewUser newUser, IJSRuntime? jsRuntime = null)
        {
            return await CreateEmailUserAsync(newUser.Email, newUser.Password, newUser.DisplayName, jsRuntime);
        }

        public static async Task<FirebaseUser?> CreateEmailUserAsync(string email, string password, string displayName, IJSRuntime? jsRuntime = null)
        {
            if (jsRuntime is not null)
            {
                StaticJsInterop = jsRuntime;
            }
            var userData = await StaticJsInterop!.InvokeAsync<string?>("window.firebaseCreateUser", email, password);
            if (string.IsNullOrEmpty(userData))
                return null;
            var newUser = await FirebaseUserFromJsonDataAsync(userData);
            if (newUser.ProviderData.Any())
            {
                newUser!.FirstProvider!.DisplayName = displayName;
            } else
            {
                newUser.ProviderData.Append(new FirebaseProviderData
                {
                    DisplayName = displayName,
                    Email = email,
                    ProviderId = "password"
                });
            }
            if (!await UpdateEmailUserDataAsync(newUser))
                return null;

            return newUser;
        }

        public static async Task<bool> UpdateEmailUserDataAsync(FirebaseUser user, IJSRuntime? jSRuntime = null)
        {
            if (!CurrentUser!.FirebaseUid.Equals(user.FirebaseUid))
            {
                return false;
            }

            if (jSRuntime is not null)
            {
                StaticJsInterop = jSRuntime;
            }

            return await StaticJsInterop!.InvokeAsync<bool>("window.firebaseUpdateProfile", user.FirstProvider);
        }

        public async Task SignOutAsync()
        {
            await this._jsRuntime!.InvokeVoidAsync("window.firebaseSignOut", this.InstanceReference);
        }
    }
}
