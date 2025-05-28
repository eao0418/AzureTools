// Group.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model
{
    using System;
    using System.Collections.Generic;

    public class Group : ModelBase
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool SecurityEnabled { get; set; }
        public bool IsAssignableToRole { get; set; }
        public List<string> GroupTypes { get; set; } = new List<string>();
        public string Visibility { get; set; } = string.Empty;
        public bool MailEnabled { get; set; }
        public List<string> Owners { get; set; } = new List<string>();
        public List<string> Members { get; set; } = new List<string>();
        public string MembershipRule { get; set; } = string.Empty;
        public string MembershipRuleProcessingState { get; set; } = string.Empty;
        public List<string> AssignedLicenses { get; set; } = new List<string>();
        public bool AllowExternalSenders { get; set; }
        public bool AutoSubscribeNewMembers { get; set; }
        public bool OnPremisesSyncEnabled { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime RenewedDateTime { get; set; }
        public List<string> ResourceBehaviorOptions { get; set; } = new List<string>();
        public string PreferredDataLocation { get; set; } = string.Empty;
    }
}
