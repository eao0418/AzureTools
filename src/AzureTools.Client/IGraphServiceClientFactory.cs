// IGraphServiceClientFactory.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    using AzureTools.Authentication.Settings;
    using Microsoft.Graph;

    public interface IGraphServiceClientFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphServiceClientFactory"/> class with the specified credential provider.
        /// </summary>
        /// <param name="credentialProvider">The <see cref="ITokenCredentialProvider"/> implementation.</param>
        GraphServiceClient AddClient(AuthenticationSettings authSettings);

        /// <summary>
        /// Retrieves a <see cref="GraphServiceClient"/> instance based on the provided client identifier.
        /// </summary>
        /// <param name="clientIdentifier">The string identifying the client.</param>
        /// <returns>The <see cref="GraphServiceClient"/>.</returns>
        GraphServiceClient GetClient(string clientIdentifier);
    }
}