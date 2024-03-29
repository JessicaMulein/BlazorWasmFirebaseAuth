﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Firebase.Components;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace Microsoft.Identity.Firebase.Models
{
    /// <summary>
    /// The Firebase implementation of <see cref="IdentityUser"/> which uses a string as a primary key.
    /// </summary>
    [Serializable]
    public class FirebaseUser : IdentityUser, IIdentity
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<FirebaseUser> manager, string authenticationType)
        {
            return StateProvider.ClaimsIdentityFromFirebaseUser(this);
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            // var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            //var userIdentity = await manager.CreateAsync(this);
            // Add custom user claims here
            //return userIdentity;
        }

        [JsonPropertyName("uid")] public string FirebaseUid { get; set; }

        /// <summary>
        /// The user's email address.
        /// Must be capitalized due to being inherited from <see cref="IdentityUser.Email"/>.
        /// </summary>
        [JsonPropertyName("email")] public override string Email { get; set; }

        [JsonPropertyName("emailVerified")] public bool EmailVerified { get; set; }

        [JsonPropertyName("isAnonymous")] public bool IsAnonymous { get; set; }

        [JsonPropertyName("providerData")] public IEnumerable<FirebaseProviderData> ProviderData { get; set; }

        public FirebaseProviderData? FirstProvider => this.ProviderData?.First();

        public string BestAvailableName
        {
            get
            {
                if (this.IsAnonymous)
                {
                    return $"Anonymous (firebase: {this.FirebaseUid})";
                }

                if (!this.ProviderData.Any())
                    return !string.IsNullOrWhiteSpace(this.Email) ? this.Email : this.FirebaseUid;

                foreach (var provider in this.ProviderData)
                {
                    if (!string.IsNullOrWhiteSpace(provider.DisplayName))
                    {
                        return provider.DisplayName;
                    }
                    else if (!string.IsNullOrWhiteSpace(provider.Email))
                    {
                        return provider.Email;
                    }
                    else if (!string.IsNullOrWhiteSpace(provider.PhoneNumber))
                    {
                        return provider.PhoneNumber;
                    }
                }
                return $"Unknown (firebase: {this.FirebaseUid})";
            }
        }

        [JsonPropertyName("stsTokenManager")] public StsTokenManager StsTokenManager { get; set; }

        [JsonPropertyName("createdAt")] public string CreatedAt { get; set; }

        [JsonPropertyName("lastLoginAt")] public string LastLoginAt { get; set; }

        /// <summary>
        /// https://firebase.google.com/docs/projects/api-keys?msclkid=50c2da1bd15411ec864a2051a4985260
        /// API keys for Firebase are different from typical API keys
        /// Unlike how API keys are typically used, API keys for Firebase services are not used to control access to backend resources; that can only be done with Firebase Security Rules(to control which users can access resources) and App Check(to control which apps can access resources).
        /// Usually, you need to fastidiously guard API keys(for example, by using a vault service or setting the keys as environment variables); however, API keys for Firebase services are ok to include in code or checked-in config files.
        /// Although API keys for Firebase services are safe to include in code, there are a few specific cases when you should enforce limits for your API key; for example, if you're using Firebase ML, Firebase Authentication with the email/password sign-in method, or a billable Google Cloud API. Learn more about these cases later on this page.
        /// </summary>
        [JsonPropertyName("apiKey")] public string ApiKey { get; set; }
        [JsonPropertyName("appName")] public string AppName { get; set; }

        public string? AuthenticationType => this.FirstProvider?.ProviderId;

        public bool IsAuthenticated => FirebaseAuth.CurrentUser is null ? false : FirebaseAuth.CurrentUser.FirebaseUid.Equals(this.FirebaseUid);


        /// <summary>
        /// Seems to be used by IIdentity users?
        /// </summary>
        public virtual string? Name => this.BestAvailableName;

        /// <summary>
        /// Initializes a new instance of <see cref="FirebaseUser"/>.
        /// </summary>
        /// <remarks>
        /// The Id property is initialized to form a new GUID string value.
        /// </remarks>
        public FirebaseUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
