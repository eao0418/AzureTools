// GraphClientSettings.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Settings
{
    using AzureTools.Utility.Settings;
    using System.Collections.Generic;

    /// <summary>
    /// A configuration class to get settings for the Graph API client.
    /// </summary>
    public class GraphClientSettings: SettingsBase
    {
        public new const string ConfigurationSectionName = "graphClientSettings";

        /// <summary>
        /// Configures the list of user properties to select in the Graph API requests.
        /// </summary>
        public List<string> UserProperties { get; init; } = new();

        /// <summary>
        /// Configures the list of service principal properties to select in the Graph API requests.
        /// </summary>
        public List<string> ServicePrincipalProperties { get; init; } = new();

        /// <summary>
        /// Configures the list of group properties to select in the Graph API requests.
        /// </summary>
        public List<string> GroupProperties { get; init; } = new();

        /// <summary>
        /// Configures the list of group member properties to select in the Graph API requests.
        /// </summary>
        public List<string> GroupMemberProperties { get; init; } = new();

        /// <summary>
        /// Configures the list of application registration properties to select in the Graph API requests.
        /// </summary>
        public List<string> ApplicationRegistrationProperties { get; init; } = new();

        /// <summary>
        /// Configures the list of application owner properties to select in the Graph API requests.
        /// </summary>
        public List<string> ApplicationOwnerProperties { get; init; } = new();

        /// <summary>
        /// Configures the list of directory role properties to select in the Graph API requests.
        /// </summary>
        public List<string> DirectoryRoleProperties { get; init; } = new();

        public string UsersEndpoint { get; set; } = string.Empty;
        public string GroupsEndpoint { get; set; } = string.Empty;
        public string ServicePrincipalsEndpoint { get; set; } = string.Empty;
        public string ApplicationRegistrationEndpoint { get; set; } = string.Empty;
        public string DirectoryRolesEndpoint { get; set; } = string.Empty;
        public string GroupMembersEndpoint { get; set; } = string.Empty;
        public string ApplicationOwnersEndpoint { get; set; } = string.Empty;
    }
}
