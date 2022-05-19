using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Identity.Firebase.Components;
using Microsoft.JSInterop;

namespace Microsoft.Identity.Firebase.Models
{
    public class FirebaseSignOutSessionStateManager : SignOutSessionStateManager
    {
        private readonly IJSRuntime jsRuntime;
        public FirebaseSignOutSessionStateManager(IJSRuntime jsRuntime) : base(jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public override ValueTask SetSignOutState()
        {
            Task.Run(async () =>
            {
                await this.jsRuntime.InvokeVoidAsync("window.firebaseSignOut", FirebaseAuth.Instance!.InstanceReference);
            });
            return new ValueTask();
        }
    }
}
