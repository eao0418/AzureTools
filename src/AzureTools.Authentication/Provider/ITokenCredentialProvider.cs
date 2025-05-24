// ITokenCredentialProvider.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Authentication.Provider
{
    using Azure.Core;
    using AzureTools.Authentication.Settings;

    /// <summary>
    /// Interface for providing token credentials.
    /// </summary>
    public interface ITokenCredentialProvider
    {
        /// <summary>
        /// Gets the default token credential.
        /// </summary>
        /// <returns></returns>
        TokenCredential GetCredential();

        /// <summary>
        /// Gets a token credential based on the provided authentication settings.
        /// </summary>
        /// <param name="settings">The <see cref="AuthenticationSettings"/>.</param>
        /// <returns>A <see cref="TokenCredential"/> matching the requested type.</returns>
        TokenCredential GetCredential(AuthenticationSettings settings);
    }
}