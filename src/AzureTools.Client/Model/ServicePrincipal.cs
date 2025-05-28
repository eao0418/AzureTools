// ServicePrincipal.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model
{
    using System;
    using System.Collections.Generic;

    public class ServicePrincipal : ModelBase
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string AppId { get; set; } = string.Empty;
        public string ApplicationType { get; set; } = string.Empty;
        public string ServicePrincipalType { get; set; } = string.Empty;
        public List<string> AppRoles { get; set; } = new List<string>();
        public List<string> Owners { get; set; } = new List<string>();
        public List<string> DelegatedPermissions { get; set; } = new List<string>();
        public bool AccountEnabled { get; set; }
        public bool PasswordCredentialsPresent { get; set; }
        public bool KeyCredentialsPresent { get; set; }
        /// <summary>
        /// MSIs associated with the service principal.
        /// </summary>
        public List<string> ManagedIdentities { get; set; } = new List<string>();
        public bool IsOauth2PermissionGrantRestricted { get; set; }
        public bool IsAssignRoleRestricted { get; set; }
        public bool IsAppRoleAssignmentRequired { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime RenewedDateTime { get; set; }
    }
}
