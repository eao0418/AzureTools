// ITokenCache.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Authentication.Cache
{
    using AzureTools.Authentication.Settings;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITokenCache
    {
        /// <summary>
        /// Gets an auth token for the Graph API.
        /// </summary>
        /// <param name="authSettings">The <see cref="AuthenticationSettings"/>.</param>
        /// <param name="stopToken">Cancellation token to cancel the operation.</param>
        public Task<string> AddTokenAsync(AuthenticationSettings authSettings, CancellationToken stopToken);

        /// <summary>
        /// Retrieves an authentication token for Graph based on the provided client identifier.
        /// </summary>
        /// <param name="clientIdentifier">The string identifying the client.</param>
        /// <param name="stopToken">Cancellation token to cancel the operation.</param>
        /// <returns>An auth token for Graph.</returns>
        public Task<string> GetTokenAsync(string clientIdentifier, CancellationToken stopToken);

        /// <summary>
        /// Gets or adds an authentication token for Graph based on the provided authentication settings.
        /// </summary>
        /// <param name="authSettings">The <see cref="AuthenticationSettings"/>.</param>
        /// <param name="stopToken">Cancellation token to cancel the operation.</param>
        /// <returns>An auth token for Graph.</returns>
        public Task<string> GetOrAddTokenAsync(AuthenticationSettings authenticationSettings, CancellationToken stopToken)
    }
}