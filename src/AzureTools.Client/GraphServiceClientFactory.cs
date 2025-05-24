// GraphServiceClientFactory.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Microsoft.Graph;
    using AzureTools.Authentication.Settings;
    using AzureTools.Authentication.Provider;

    /// <summary>
    /// Factory class for creating and managing instances of <see cref="GraphServiceClient"/>.
    /// </summary>
    public class GraphServiceClientFactory : IGraphServiceClientFactory
    {
        private ConcurrentDictionary<string, GraphServiceClient> _clients = new ConcurrentDictionary<string, GraphServiceClient>();
        private ITokenCredentialProvider _credentialProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphServiceClientFactory"/> class with the specified credential provider.
        /// </summary>
        /// <param name="credentialProvider">The <see cref="ITokenCredentialProvider"/> implementation.</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="credentialProvider"/> is null.</exception>
        public GraphServiceClientFactory(ITokenCredentialProvider credentialProvider)
        {
            _credentialProvider = credentialProvider ?? throw new ArgumentNullException(nameof(credentialProvider));
        }

        /// <summary>
        /// Retrieves a <see cref="GraphServiceClient"/> instance based on the provided client identifier.
        /// </summary>
        /// <param name="clientIdentifier">The string identifying the client.</param>
        /// <returns>The <see cref="GraphServiceClient"/>.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="clientIdentifier"/> is null, empty, or whitespace.</exception>
        /// <exception cref="KeyNotFoundException">If the <paramref name="clientIdentifier"/> does not correspond to a client.</exception>
		public GraphServiceClient GetClient(string clientIdentifier)
        {
            if (string.IsNullOrWhiteSpace(clientIdentifier))
            {
                throw new ArgumentNullException(nameof(clientIdentifier));
            }
            if (_clients.TryGetValue(clientIdentifier, out var client))
            {
                return client;
            }

            throw new KeyNotFoundException($"No client exists for {clientIdentifier}");
        }

        /// <summary>
        /// Adds a new <see cref="GraphServiceClient"/> instance to the factory using the provided authentication settings and client.
        /// </summary>
        /// <param name="authSettings">The <see cref="AuthenticationSettings"/>.</param>
        /// <exception cref="Exception">If the <see cref="GraphServiceClient"/> cannot be created or added to the factory.</exception>
		public GraphServiceClient AddClient(AuthenticationSettings authSettings)
        {
            ArgumentNullException.ThrowIfNull(authSettings);

            var scopes = new[] { $"{authSettings.Audience}.default" };

            var credential = _credentialProvider.GetCredential(authSettings);

            var graphClient = new GraphServiceClient(credential, scopes);

            if (graphClient == null)
            {
                throw new Exception($"Failed to create GraphServiceClient for {authSettings.GetAuthKey()}");
            }

            if (!_clients.TryAdd(authSettings.GetAuthKey(), graphClient))
            {
                throw new Exception($"Failed to add GraphServiceClient for {authSettings.GetAuthKey()}");
            }

            return graphClient;
        }
    }
}
