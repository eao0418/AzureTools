// TokenCache.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Authentication.Cache
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using AzureTools.Authentication.Settings;
    using AzureTools.Authentication.Provider;
    using System.Threading;
    using Azure.Core;
    using System.Threading.Tasks;

    /// <summary>
    /// Factory class for creating and managing instances of <see cref="AccessToken"/>.
    /// </summary>
    public class TokenCache : ITokenCache
    {
        private ConcurrentDictionary<string, AccessToken> _tokens = new ConcurrentDictionary<string, AccessToken>();
        private ConcurrentDictionary<string, AuthenticationSettings> _settingsCache = new ConcurrentDictionary<string, AuthenticationSettings>();
        private ITokenCredentialProvider _credentialProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenCache"/> class with the specified credential provider.
        /// </summary>
        /// <param name="credentialProvider">The <see cref="ITokenCredentialProvider"/> implementation.</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="credentialProvider"/> is null.</exception>
        public TokenCache(ITokenCredentialProvider credentialProvider)
        {
            _credentialProvider = credentialProvider ?? throw new ArgumentNullException(nameof(credentialProvider));
        }

        /// <summary>
        /// Retrieves an authentication token for Graph based on the provided client identifier.
        /// </summary>
        /// <param name="clientIdentifier">The string identifying the client.</param>
        /// <param name="stopToken">Cancellation token to cancel the operation.</param>
        /// <returns>An auth token for Graph.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="clientIdentifier"/> is null, empty, or whitespace.</exception>
        /// <exception cref="KeyNotFoundException">If the <paramref name="clientIdentifier"/> does not correspond to a client.</exception>
		public async Task<string> GetTokenAsync(string clientIdentifier, CancellationToken stopToken)
        {
            if (string.IsNullOrWhiteSpace(clientIdentifier))
            {
                throw new ArgumentNullException(nameof(clientIdentifier));
            }
            if (_tokens.TryGetValue(clientIdentifier, out var existingToken))
            {
                if (existingToken.ExpiresOn <= DateTimeOffset.UtcNow.AddMinutes(-5))
                {
                    if(_settingsCache.TryGetValue(clientIdentifier, out var setting))
                    {
                        var newToken = await GetAccessTokenAsync(setting, stopToken);
                        if (!_tokens.TryUpdate(clientIdentifier, existingToken, newToken))
                        {
                            _tokens.TryRemove(clientIdentifier, out _);
                            throw new Exception($"Failed to update token for {clientIdentifier}");
                        }

                        return newToken.Token;
                    }
                }

                return existingToken.Token;
            }

            throw new KeyNotFoundException($"No client exists for {clientIdentifier}");
        }

        /// <summary>
        /// Gets an auth token for the Graph API.
        /// </summary>
        /// <param name="authSettings">The <see cref="AuthenticationSettings"/>.</param>
        /// <param name="stopToken">Cancellation token to cancel the operation.</param>
        /// <returns>An auth token for Graph.</returns>
        /// <exception cref="Exception">If the <see cref="AccessToken"/> cannot be created or added to the cache.</exception>
		public async Task<string> AddTokenAsync(AuthenticationSettings authSettings, CancellationToken stopToken)
        {
            ArgumentNullException.ThrowIfNull(authSettings);

            var accessToken = await GetAccessTokenAsync(authSettings, stopToken);

            _settingsCache.TryAdd(authSettings.GetAuthKey(), authSettings);

            if (!_tokens.TryAdd(authSettings.GetAuthKey(), accessToken))
            {
                throw new Exception($"Failed to add an AccessToken for {authSettings.GetAuthKey()}");
            }

            return accessToken.Token;
        }

        /// <summary>
        /// Gets or adds an authentication token for Graph based on the provided authentication settings.
        /// </summary>
        /// <param name="authSettings">The <see cref="AuthenticationSettings"/>.</param>
        /// <param name="stopToken">Cancellation token to cancel the operation.</param>
        /// <returns>An auth token for Graph.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="authSettings"/> is null.</exception>
        public async Task<string> GetOrAddTokenAsync(AuthenticationSettings authSettings, CancellationToken stopToken)
        {
            ArgumentNullException.ThrowIfNull(authSettings);
            var authKey = authSettings.GetAuthKey();

            string token;
            try
            {
                token = await GetTokenAsync(authKey, stopToken);
            }
            catch (KeyNotFoundException)
            {
                // If the token is not found, we will add it.
                token = await AddTokenAsync(authSettings, stopToken);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is KeyNotFoundException)
            {
                // If we have an error retrieving the token, we will try to add it.
                token = await AddTokenAsync(authSettings, stopToken);
            }

            return token;
        }

        private async Task<AccessToken> GetAccessTokenAsync(AuthenticationSettings authSettings, CancellationToken stopToken)
        {
            var scopes = new[] { $"{authSettings.Audience}.default" };

            var credential = _credentialProvider.GetCredential(authSettings);

            return await credential.GetTokenAsync(new Azure.Core.TokenRequestContext(scopes), stopToken);
        }
    }
}
