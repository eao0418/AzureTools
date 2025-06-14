// AuthenticationSettings.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Authentication.Settings
{
    using AzureTools.Utility.Settings;

    /// <summary>
    /// Represents the authentication settings for the Azure Tools library.
    /// </summary>
    public class AuthenticationSettings: SettingsBase
    {
        public new const string ConfigurationSectionName = "authenticationSettings";

        /// <summary>
        /// The authentication type to use. The default is <see cref="AuthenticationType.Default"/>.
        /// </summary>
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Default;

        /// <summary>
        /// The Client ID of the application registered in Entra (AAD).
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// The Entra (AAD) tenant ID. 
        /// </summary>
        public string TenantId { get; set; } = string.Empty;

        /// <summary>
        /// The audience of the request. 
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// The UserName for human user authentication. This is only used if the authentication type is set to <see cref="AuthenticationType.UsernamePassword"/>.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// The password for the user. This is only used if the authentication type is set to <see cref="AuthenticationType.UsernamePassword"/>.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Represents the location for a secret. This could be a URL for a secret in KeyVault.
        /// </summary>
        public string SecretLocation { get; set; } = string.Empty;

        /// <summary>
        /// Generates a string that can be used to represent the client that belongs to this authentication settings object.
        /// </summary>
        /// <returns>A <see cref="string"/>.</returns>
        public string GetAuthKey()
        {
            return $"{TenantId}-{ClientId}-{UserName}-{Audience}-{AuthenticationType}";
        }
    }
}
