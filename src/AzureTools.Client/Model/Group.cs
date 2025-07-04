﻿// Group.cs Copyright (c) Aaron Randolph. All rights reserved.
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
        public string IsAssignableToRole { get; set; } = string.Empty;
        public List<string> GroupTypes { get; set; } = [];
        public string Visibility { get; set; } = string.Empty;
        public bool MailEnabled { get; set; }
        public string MembershipRule { get; set; } = string.Empty;
        public string MembershipRuleProcessingState { get; set; } = string.Empty;
        public bool AllowExternalSenders { get; set; }
        public bool? AutoSubscribeNewMembers { get; set; }
        public bool? OnPremisesSyncEnabled { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime RenewedDateTime { get; set; }
        public List<string> ResourceBehaviorOptions { get; set; } = [];
        public string PreferredDataLocation { get; set; } = string.Empty;
        public string OnPremisesDomainName { get; set; } = string.Empty;
        public DateTime? OnPremisesLastSyncDateTime { get; set; }
        public string OnPremisesNetBiosName { get; set;} = string.Empty;
        public string OnPremisesSamAccountName { get; set;} = string.Empty;
        public string OnPremisesSecurityIdentifier { get; set; } = string.Empty;
    }
}
