// ServicePrincipal.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model.Graph
{
    using System;
    using System.Collections.Generic;

    public class ServicePrincipal : ModelBase
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string AppId { get; set; } = string.Empty;
        public string ServicePrincipalType { get; set; } = string.Empty;
        public List<object> AppRoles { get; set; } = [];
        public bool AccountEnabled { get; set; }
        public bool IsAssignRoleRestricted { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("appRoleAssignmentRequired")]
        public bool IsAppRoleAssignmentRequired { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime RenewedDateTime { get; set; }
    }
}
