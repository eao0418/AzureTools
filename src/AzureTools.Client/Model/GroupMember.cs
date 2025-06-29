// GroupMember.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class GroupMember: ModelBase
    {
        /// <summary>
        /// Group member uuid
        /// </summary>
        public string Id { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        [JsonPropertyName("@odata.type")]
        public string ObjectType { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string UserPrincipalName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool AccountEnabled { get; set; } 
        public List<string> AssignedRoles { get; set; } = []; 
        public List<string> Devices { get; set; } = []; 
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastSignInDateTime { get; set; }
    }
}