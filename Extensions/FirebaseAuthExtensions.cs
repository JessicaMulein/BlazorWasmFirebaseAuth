using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Firebase.Components;
using Microsoft.Identity.Firebase.Models;

namespace Microsoft.Identity.Firebase.Extensions
{
    public static class FirebaseAuthExtensions
    {
        public static async Task<WebAssemblyHostBuilder> UseFirebaseAuthenticationAsync(this WebAssemblyHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            };
            var projectConfiguration = builder.Configuration.GetRequiredSection(FirebaseProjectConfiguration.ConfigSectionName);
            var uiConfiguration = builder.Configuration.GetRequiredSection(FirebaseUiConfiguration.ConfigSectionName);
            var openIdConfiguration = await FirebaseOpenIdConfiguration.GetFirebaseOpenIdConfigurationAsync(
                    configurationUrl: string.Join(httpClient.BaseAddress.ToString(), "/openIdConfiguration.json"),
                    httpClient: httpClient);

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });
            builder.Services.AddScoped(sp => httpClient);
            builder.Services.AddSingleton<FirebaseOpenIdConfiguration>(implementationInstance: openIdConfiguration);
            builder.Services.AddSingleton<FirebaseProjectConfiguration>(implementationInstance: new FirebaseProjectConfiguration(projectConfiguration, openIdConfiguration));
            builder.Services.AddSingleton<FirebaseUiConfiguration>(implementationInstance: new FirebaseUiConfiguration(uiConfiguration));
            builder.Services.AddSingleton<FirebaseAuth>(implementationInstance: new FirebaseAuth());
            builder.Services.AddSingleton<StateProvider>();
            builder.Services.AddScoped<IdentityUser, FirebaseUser>();
            builder.Services.AddSingleton<AuthenticationStateProvider>(s => s.GetRequiredService<StateProvider>());
            builder.Services.AddSingleton<SignOutSessionStateManager, FirebaseSignOutSessionStateManager>();

            return builder;
        }
    }
}
