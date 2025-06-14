// AuthenticationType.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Authentication.Settings
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Enumeration of authentication types.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AuthenticationType
    {
        /// <summary>
        /// Use the default authentication method.
        /// </summary>
        Default,
        /// <summary>
        /// Use the interactive authentication method.
        /// </summary>
        Interactive,
        /// <summary>
        /// Use the device code authentication method.
        /// </summary>
        DeviceCode,
        /// <summary>
        /// Use the managed identity authentication method.
        /// </summary>
        ManagedIdentity,
        /// <summary>
        /// Use the client secret authentication method.
        /// </summary>
        ClientSecret,
        /// <summary>
        /// Authentication using username and password.
        /// </summary>
        UsernamePassword,
        VisualStudio
    }
}
