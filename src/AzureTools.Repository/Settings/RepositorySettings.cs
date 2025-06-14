// RepositorySettings.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Repository.Settings
{
    using AzureTools.Utility.Settings;

    public sealed class RepositorySettings : SettingsBase
    {
        public new const string ConfigurationSectionName = "repositorySettings";

        /// <summary>
        /// Gets or sets the repository type.
        /// </summary>
        public RepositoryType RepositoryType { get; set; }

        /// <summary>
        /// Gets or sets the database name for the repository.
        /// </summary>
        public string? DatabaseName { get; set; }
    }
}
