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

            _ = Task.Run(async () =>
            {
                await base.SetSignOutState();

                var signedOut = await this.jsRuntime.InvokeAsync<bool>("window.firebaseSignOut", FirebaseAuth.Instance!.InstanceReference);
                if (signedOut is true)
                {
                    await FirebaseAuth.Instance!.OnAuthStateChanged(null);
                }
            });
            return new ValueTask();
        }
    }
}
