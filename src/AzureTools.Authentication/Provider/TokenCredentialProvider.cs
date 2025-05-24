// TokenCredentialProvider.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Authentication.Provider
{
    using Azure.Identity;
    using Azure.Core;
    using AzureTools.Authentication.Settings;
    using System.Collections.Concurrent;

    /// <summary>
    /// Provides token credentials based on the specified authentication settings.
    /// </summary>
    public class TokenCredentialProvider : ITokenCredentialProvider
    {
        /// <summary>
        /// A dictionary to hold the credential factory methods for different authentication types.
        /// </summary>
        private readonly ConcurrentDictionary<AuthenticationType, Func<AuthenticationSettings, TokenCredential>> _credentialFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenCredentialProvider"/> class.
        /// </summary>
        public TokenCredentialProvider()
        {
            _credentialFactory = new ConcurrentDictionary<AuthenticationType, Func<AuthenticationSettings, TokenCredential>>();
            Initialize();
        }

        /// <inheritdoc/>
        public TokenCredential GetCredential()
        {
            var options = new DefaultAzureCredentialOptions()
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ExcludeManagedIdentityCredential = true,
            };

            return new DefaultAzureCredential(options);
        }

        /// <inheritdoc/>
        public TokenCredential GetCredential(AuthenticationSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            return GetCredentialImpl(settings);
        }

        private TokenCredential GetCredentialImpl(AuthenticationSettings settings)
        {
            if (settings.AuthenticationType == AuthenticationType.Default)
            {
                return GetCredential();
            }

            if (_credentialFactory.TryGetValue(settings.AuthenticationType, out var initializer))
            {
                return initializer.Invoke(settings);
            }

            throw new NotSupportedException($"Authentication type '{settings.AuthenticationType}' is not supported.");
        }

        /// <summary>
        /// Creates a <see cref="TokenCredential"/> using the Managed Identity authentication method.
        /// </summary>
        /// <param name="settings">the <see cref="AuthenticationSettings"/>.</param>
        /// <returns>A <see cref="TokenCredential"/> for the Managed Identity.</returns>
        private TokenCredential GetManagedIdentityCredential(AuthenticationSettings settings)
        {
            var options = new ManagedIdentityCredentialOptions()
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };
            return new ManagedIdentityCredential(options);
        }

        /// <summary>
        /// Creates a <see cref="TokenCredential"/> using the Client Secret authentication method.
        /// </summary>
        /// <param name="settings">The <see cref="AuthenticationSettings"/>.</param>
        /// <returns>A <see cref="TokenCredential"/>.</returns>
        private TokenCredential GetClientSecretCredential(AuthenticationSettings settings)
        {
            var options = new ClientSecretCredentialOptions()
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            };
            return new ClientSecretCredential(settings.TenantId, settings.ClientId, "placeholder", options);
        }

#pragma warning disable CS0618 // CS0618: Type or member is obsolete. The use here is intentional.
        /// <summary>
        /// Creates a <see cref="TokenCredential"/> using the Username and Password authentication method.
        /// </summary>
        /// <param name="settings">The <see cref="AuthenticationSettings"/>.</param>
        /// <returns>A <see cref="TokenCredential"/>.</returns>
        private TokenCredential GetUsernamePasswordCredential(AuthenticationSettings settings)
        {
            return new UsernamePasswordCredential(settings.ClientId, settings.UserName, settings.Password, settings.TenantId);
        }
#pragma warning restore CS0618 // CS0618: Type or member is obsolete. The use here is intentional.

        private TokenCredential GetDeviceCodeCredential(AuthenticationSettings settings)
        {
            var options = new DeviceCodeCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ClientId = settings.ClientId,
                TenantId = settings.TenantId,
                // Callback function that receives the user prompt
                // Prompt contains the generated device code that user must
                // enter during the auth process in the browser
                DeviceCodeCallback = (code, cancellation) =>
                {
                    Console.WriteLine(code.Message);
                    return Task.FromResult(0);
                },
            };

            return new DeviceCodeCredential(options);
        }

        /// <summary>
        /// Initializes the credential factory with the supported authentication types and their corresponding credential creation methods.
        /// </summary>
        private void Initialize()
        {
            _credentialFactory.TryAdd(AuthenticationType.ManagedIdentity, new Func<AuthenticationSettings, TokenCredential>(GetManagedIdentityCredential));
            _credentialFactory.TryAdd(AuthenticationType.ClientSecret, new Func<AuthenticationSettings, TokenCredential>(GetClientSecretCredential));
            _credentialFactory.TryAdd(AuthenticationType.UsernamePassword, new Func<AuthenticationSettings, TokenCredential>(GetUsernamePasswordCredential));
            _credentialFactory.TryAdd(AuthenticationType.DeviceCode, new Func<AuthenticationSettings, TokenCredential>(GetDeviceCodeCredential));
        }
    }
}
